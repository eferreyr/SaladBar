package snhp.asu.edu.saladbar;

/**
 * Created by John on 8/15/17.
 */

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.support.annotation.NonNull;
import android.util.Log;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;

import okhttp3.MediaType;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;
import snhp.asu.edu.saladbar.Models.Device;
import snhp.asu.edu.saladbar.Models.InterventionDay;
import snhp.asu.edu.saladbar.Models.RandomizedStudent;
import snhp.asu.edu.saladbar.Models.RandomizedStudentTray;
import snhp.asu.edu.saladbar.Models.ResearchTeamMember;
import snhp.asu.edu.saladbar.Models.School;
import snhp.asu.edu.saladbar.Models.Staging.RandomizedStudentStaging;
import snhp.asu.edu.saladbar.Models.Staging.RandomizedStudentTrayStaging;
import snhp.asu.edu.saladbar.Models.Token;


public class SyncTask extends AsyncTask<String, Void, String> {

    private static final String TAG = "asu-snhp";

    private OkHttpClient client = new OkHttpClient();

    private ProgressDialog mProgressSpinner = null;
    private Context ctx;
    private String email;
    private String password;

    private String REGULAR_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
    private String REGULAR_TIME_WITH_MILLISECONDS_FORMAT = "yyyy-MM-dd'T'HH:mm:ss.SSS";

    private SimpleDateFormat dateFormatter = new SimpleDateFormat(REGULAR_TIME_FORMAT);

    private final String BASE_URL = "https://saladbar.snhp.asu.edu";
    private final String TOKEN_PATH = BASE_URL + "/account/generatetoken";
    private final String RESEARCH_TEAM_MEMBER_PATH = BASE_URL + "/api/Sync/ResearchTeamMember";
    private final String SCHOOL_PATH = BASE_URL + "/api/Sync/School?includeLogo=";
    private final String INTERVENTION_DAY_PATH = BASE_URL + "/api/Sync/InterventionDay";
    private final String RANDOMIZED_STUDENT_PATH = BASE_URL + "/api/Sync/RandomizedStudent";
    private final String RANDOMIZED_STUDENT_STAGING_PATH = BASE_URL + "/api/Sync/RandomizedStudentStaging";
    private final String RANDOMIZED_STUDENT_WITH_INTERVENTION_DAY_ID_PATH = BASE_URL + "/api/Sync/RandomizedStudentByInterventionDayId?intervention_day_id=";
    private final String RANDOMIZED_STUDENT_TRAY_STAGING_PATH = BASE_URL + "/api/Sync/RandomizedStudentTrayStaging ";

    private final String DEVICE_PATH = BASE_URL + "/api/Sync/Device";
    private final String BATCH_ID_PATH = BASE_URL + "/api/Sync/Batch";

    private final MediaType JSON_CONTENT_TYPE = MediaType.parse("application/json; charset=utf-8");

    private final String RESPONSE_TOKEN_KEY = "token";
    private final String RESPONSE_TOKEN_EXPIRE_KEY = "expires";

    public SyncTask(Context ctx, String email, String password) {
        this.ctx = ctx;
        this.email = email;
        this.password = password;
    }

    @Override
    protected void onPreExecute() {
        mProgressSpinner = new ProgressDialog(ctx);
        mProgressSpinner.setMessage("Syncing...");
        mProgressSpinner.setProgressStyle(ProgressDialog.STYLE_SPINNER);
        mProgressSpinner.setCancelable(false);
        mProgressSpinner.show();
        super.onPreExecute();
    }

    @Override
    protected String doInBackground(String... params) {

        // TODO: Add code to properly catch error during each of the syncs.
        Token token = requestToken();
        String deviceId;
        long batchId;
        if(token != null)
        {
            // Sync Device
            deviceId = syncDevice(token);
            if (!deviceId.isEmpty())
            {
                // Get Batch Id
                batchId = getBatchId(token);
                if (batchId > 0)
                {
                    // Get the most up-to-date information for research team members,
                    // schools, and intervention days
                    if (!syncResearchTeamMembers(token)) return "Sync Failed: Failed while trying to get research team members.";
                    if (!syncSchools(true, token)) return "Sync Failed: Failed while trying to get schools.";
                    if (!syncInterventionDays(token)) return "Sync Failed: Failed while trying to get intervention days.";

                    // Push new data from tablet up to the server first, then grab the most
                    // up-to-date information from the server
                    if (!syncRandomizedStudents(token, batchId, deviceId)) return "Sync Failed: Failed while trying to sync randomized students.";
                    if (!syncRandomizedStudentTrays(token, batchId, deviceId)) return "Sync Failed: Failed while trying to sync randomized student trays.";

                    // TODO: Sync Audit and possible Tracking.

                    return "Sync Successful";
                }

                else
                {
                    // Report issue
                    return "Sync Failed: Invalid batch id";
                }
            }

            else
            {
                // Report issue
                return "Sync Failed: Device id not found";
            }
        }
        else {
            return "Sync Failed: Invalid token";
        }
    }

    @Override
    protected void onPostExecute(String result) {

        try {
            mProgressSpinner.dismiss();
            Toast.makeText(ctx, result, Toast.LENGTH_LONG).show();
        } catch (Exception e) {
            Log.e("ab", "Weird error on dismiss of progress spinner.", e);
        }

        mProgressSpinner = null;
        super.onPostExecute(result);

        if (result.equalsIgnoreCase("Sync Successful")) {
            Activity activity = (Activity) ctx;
            activity.finish();
        }
    }

    private Token requestToken()
    {
        RequestBody requestBody = RequestBody.create(JSON_CONTENT_TYPE, "{\"email\":\"" + this.email + "\"" + ", \"password\":\"" + this.password + "\"}");

        // Request token first before we can do anything else.
        Request request = new Request.Builder()
                .url(TOKEN_PATH)
                .post(requestBody)
                .build();

        try {
            Response response = client.newCall(request).execute();

            if(response.code() == 200)
            {
                String responseJson = response.body().string();

                Gson gson = new GsonBuilder()
                    .setDateFormat(REGULAR_TIME_FORMAT)
                    .create();
                Token token = gson.fromJson(responseJson, Token.class);
                token.setEmail(this.email);
                token.setCreatedBy(this.email);
                DbAdapter.insertToken(token);

                return token;
            }
        }catch (Exception e){
            Log.e(TAG, "Error while requesting token: ", e);
            return null;
        }

        return null;
    }

    // This method returns an empty string, it means something went wrong during the sync. Otherwise,
    // it'll return the actual deviceId.
    @NonNull
    private String syncDevice(Token token)
    {
        try {
            Device device = DbAdapter.getDevice();

            if (device != null) {
                Gson gson = new GsonBuilder()
                        .registerTypeAdapter(Date.class, new DateDeserializer())
                        .create();

                RequestBody requestBody = RequestBody.create(JSON_CONTENT_TYPE, gson.toJson(device));

                Request request = new Request.Builder()
                        .url(DEVICE_PATH)
                        .post(requestBody)
                        .header("Authorization", "Bearer " + token.getToken())
                        .build();

                Response response = client.newCall(request).execute();

                if (response.code() == 200) {
                    String responseJson = response.body().string();

                    Device returnedDevice = gson.fromJson(responseJson, Device.class);

                    if (device.getDeviceId().equals(returnedDevice.getDeviceId()) && device.getDeviceName().equals(returnedDevice.getDeviceName()))
                    {
                        return device.getDeviceId();
                    }
                    else
                    {
                        // Print what went wrong and return empty string
                        Log.e(TAG, "Device in db does not match the return device");
                        return "";
                    }

                }
                else {
                    // Print the error code from server and return empty string
                    Log.e(TAG, "Error while trying to push device to server: " + response.message() + " " + response.body().string() + "\n" + response.toString());
                    return "";
                }
            }
            else {
                // Print what went wrong and return empty string
                Log.e(TAG, "Device is not found in db");
                return "";
            }
        }catch (Exception e){
            Log.e(TAG, "Sync Device Error: " ,e);
            return "";
        }
    }

    private long getBatchId(Token token)
    {
        long batchId = -1;
        try {
            Device device = DbAdapter.getDevice();

            if (device != null) {
                Gson gson = new Gson();

                RequestBody requestBody = RequestBody.create(JSON_CONTENT_TYPE, "{\"deviceId\":\"" + device.getDeviceId() + "\"" + "}");

                Request request = new Request.Builder()
                        .url(BATCH_ID_PATH)
                        .post(requestBody)
                        .header("Authorization", "Bearer " + token.getToken())
                        .build();

                Response response = client.newCall(request).execute();

                if (response.code() == 200) {
                    String responseStr = response.body().string();
                    batchId = Integer.parseInt(responseStr);

                    return batchId;
                }
                else {
                    // Print the error code from server and return -1
                    Log.e(TAG, "Error while trying to get batch id from server: " + response.message() + " " + response.body().string() + "\n" + response.toString());
                    return batchId;
                }
            }
            else {
                // Print what went wrong and return -1
                Log.e(TAG, "Device is not found in db");
                return batchId;
            }
        }catch (Exception e){
            Log.e(TAG, "Error while getting batch id: ", e);
            return batchId;
        }
    }


    private boolean syncResearchTeamMembers(Token token)
    {
        try {
            Request request = new Request.Builder()
                    .url(RESEARCH_TEAM_MEMBER_PATH)
                    .get()
                    .header("Authorization", "Bearer " + token.getToken())
                    .build();

            Response response = client.newCall(request).execute();

            if(response.code() == 200)
            {
                String responseJson = response.body().string();

                Gson gson = new GsonBuilder()
                        .setDateFormat(REGULAR_TIME_WITH_MILLISECONDS_FORMAT)
                        .create();
                ResearchTeamMember[] researchTeamMembers = gson.fromJson(responseJson, ResearchTeamMember[].class);

                DbAdapter.insertResearcherTeamMembers(researchTeamMembers);

                return true;
            }
            else {
                // Print the error code from server and return false
                Log.e(TAG, "Error while trying to get research team members from server: " + response.message() + " " + response.body().string() + "\n" + response.toString());
                return false;
            }
        }catch (Exception e){
            Log.e(TAG, "Error while syncing research team members: ", e);
            return false;
        }
    }

    private boolean syncSchools(boolean includeLogo, Token token)
    {
        try {
            Request request = new Request.Builder()
                    .url(SCHOOL_PATH + includeLogo)
                    .get()
                    .header("Authorization", "Bearer " + token.getToken())
                    .build();

            Response response = client.newCall(request).execute();

            if(response.code() == 200)
            {
                String responseJson = response.body().string();

                Gson gson = new GsonBuilder()
                        .setDateFormat(REGULAR_TIME_WITH_MILLISECONDS_FORMAT)
                        .registerTypeHierarchyAdapter(byte[].class, new Base64ToByteArrayTypeAdapter())
                        .create();
                School[] schools = gson.fromJson(responseJson, School[].class);
                DbAdapter.insertSchools(schools);

                return true;
            }
            else {
                // Print the error code from server and return false
                Log.e(TAG, "Error while trying to get schools from server: " + response.message() + " " + response.body().string() + "\n" + response.toString());
                return false;
            }
        }catch (Exception e){
            Log.e(TAG, "Error while syncing schools: ", e);
            return false;
        }
    }

    private boolean syncInterventionDays(Token token)
    {
        try {
            Request request = new Request.Builder()
                .url(INTERVENTION_DAY_PATH)
                .get()
                .header("Authorization", "Bearer " + token.getToken())
                .build();

            Response response = client.newCall(request).execute();

            if(response.code() == 200)
            {
                String responseJson = response.body().string();

                Gson gson = new GsonBuilder()
                        .registerTypeAdapter(Date.class, new DateDeserializer())
                        .create();
                InterventionDay[] interventionDays = gson.fromJson(responseJson, InterventionDay[].class);
                DbAdapter.insertInterventionDays(interventionDays);

                return true;
            }
            else {
                // Print the error code from server and return false
                Log.e(TAG, "Error while trying to get intervention days from server: " + response.message() + " " + response.body().string() + "\n" + response.toString());
                return false;
            }
        }catch (Exception e){
            Log.e(TAG, "Error while syncing intervention days: ", e);
            return false;
        }
    }

    private boolean syncRandomizedStudents(Token token, long batchId, String deviceId)
    {
        if (pushRandomizedStudents(token, batchId, deviceId))
        {
            if (pullRandomizedStudents(token))
            {
                return true;
            }

            else
            {
                Log.e(TAG,"Failed to pull all randomized students from server");
                return false;
            }
        }

        else
        {
            Log.e(TAG,"Failed to push all randomized students to server");
            return false;
        }
    }

    private boolean pushRandomizedStudents(Token token, long batchId, String deviceId)
    {
        int dirtyRowCount = DbAdapter.countDirtyRowsForTable(DbAdapter.RANDOMIZED_STUDENT_TABLE);
        int loopCount = 0;

        if (dirtyRowCount > 0) {

            do {
                RandomizedStudent rs = (RandomizedStudent) DbAdapter.getDirtyRowForTable(DbAdapter.RANDOMIZED_STUDENT_TABLE);

                // If the return value is not null, go ahead and serialize it to JSON and send up to server.
                if (rs != null) {

                    try {
                        if (!specialTemporaryMethodForSettingDirtyClean(rs)) {
                            continue;
                        }

                        // Ignoring TEST HIGH SCHOOL and TEST ELEMENTARY SCHOOL's data
                        if (rs.getId() <= 0) {
                            // Since we don't care about the TEST school's data, we are just going to mark
                            // them as cleaned.
                            DbAdapter.setRowClean(DbAdapter.RANDOMIZED_STUDENT_TABLE, rs.getId());
                            continue;
                        }

                        // Convert randomized student to the randomized student staging for easier
                        // upload to server.
                        RandomizedStudentStaging rss = new RandomizedStudentStaging(batchId, deviceId, rs);

                        // Convert to JSON
                        Gson gson = new GsonBuilder()
                                .registerTypeAdapter(Date.class, new DateDeserializer())
                                .create();

                        // Send it off to server
                        RequestBody requestBody = RequestBody.create(JSON_CONTENT_TYPE, gson.toJson(rss));

                        Request request = new Request.Builder()
                                .url(RANDOMIZED_STUDENT_STAGING_PATH)
                                .post(requestBody)
                                .header("Authorization", "Bearer " + token.getToken())
                                .build();

                        Response response = client.newCall(request).execute();

                        if (response.code() == 200) {
                            String responseJson = response.body().string();

                            RandomizedStudentStaging returnedRss = gson.fromJson(responseJson, RandomizedStudentStaging.class);

                            // Verify the return of the server
                            if (rss.compareTo(returnedRss)) {
                                // If the return value is valid, go ahead and mark it as complete
                                DbAdapter.setRowClean(DbAdapter.RANDOMIZED_STUDENT_TABLE, rs.getId());
                            } else {
                                // Print what went wrong and return false;
                                Log.e(TAG, "Error: Randomized student from tablet is not the same as the server's.");
                                return false;
                            }

                        } else {
                            // Print the error code from server and return false
                            Log.e(TAG, "Error while trying to push randomized students to server: " + response.message() + " " + response.body().string() + "\n" + response.toString());
                            return false;
                        }
                    } catch (Exception e) {
                        // Print the error message and return false
                        Log.e(TAG, "Error while trying to push randomized students to server: ", e);
                        return false;
                    } finally {
                        loopCount++;
                    }
                } else {
                    Log.e(TAG, "Found a null randomized student.");
                    break;
                }

            } while (loopCount < dirtyRowCount);

            dirtyRowCount = DbAdapter.countDirtyRowsForTable(DbAdapter.RANDOMIZED_STUDENT_TABLE);

            Log.i(TAG, "Dirty Row Count for " + DbAdapter.RANDOMIZED_STUDENT_TABLE + " after pushing: " + dirtyRowCount);

            return dirtyRowCount == 0;
        }
        else {
            return true;
        }
    }

    private boolean pullRandomizedStudents(Token token)
    {
        // Get all unfinished intervention day first
        ArrayList<InterventionDay> itds = DbAdapter.getAllUnfinishedInterventionDays();

        // Go through all intervention days and get randomized student for those days
        for(InterventionDay itd : itds)
        {
            Calendar cal = Calendar.getInstance();
            cal.add(Calendar.DAY_OF_YEAR, -1);
            Date yesterday = cal.getTime();

            if (itd.getInterventionDate().after(yesterday)) {
                boolean pullResult = getRandomizedStudentsForInterventionDay(itd.getId(), token);
                // If one of the GET fails, continue, but logging it.
                if (!pullResult) {
                    Log.e("Error", "Failed to get randomized students for " + itd.getId());
                }
            }
        }

        return true;
    }

    private boolean getRandomizedStudentsForInterventionDay(int interventionId, Token token)
    {
        // Go in the db and grab all active intervention day ids and start downloading randomized students
        try {
            Request request = new Request.Builder()
                    .url(RANDOMIZED_STUDENT_WITH_INTERVENTION_DAY_ID_PATH + interventionId)
                    .get()
                    .header("Authorization", "Bearer " + token.getToken())
                    .build();

            Response response = client.newCall(request).execute();

            if(response.code() == 200)
            {
                String responseJson = response.body().string();

                Gson gson = new GsonBuilder()
                        .registerTypeAdapter(Date.class, new DateDeserializer())
                        .create();
                RandomizedStudent[] randomizedStudents = gson.fromJson(responseJson, RandomizedStudent[].class);
                DbAdapter.insertRandomizedStudents(randomizedStudents);

                return true;
            }

            else {
                // Print the error code from server and return false
                Log.e(TAG, "Error while trying to pull randomized students from server: " + response.message() + " " + response.body().string() + "\n" + response.toString());
                return false;
            }
        }
        catch (Exception e){
            Log.e(TAG, "Error while getting randomized students for intervention days: ", e);
            return false;
        }
    }

    // This method is intend to live temporary. Prior to sync functionality, randomized students were added by code
    // manually. At that time, I set all new randomized students' dirty flag to 'Y' on first insert when it really
    // isn't dirty. And the server check all incoming randomized students' assent value, null value are not accepted.
    // So this method basically set the dirty flag to 'N' for randomized students that have a 'null' as their assent value.
    // True means is truly dirty, false mean it's not dirty continue on to the next randomized student.
    private boolean specialTemporaryMethodForSettingDirtyClean(RandomizedStudent rs)
    {
        if (rs.getDirty().equalsIgnoreCase("Y")) {
            if (rs.getAssent() == null) {
                DbAdapter.setRowClean(DbAdapter.RANDOMIZED_STUDENT_TABLE, rs.getId());
                return false;
            }
            else {
                return true;
            }
        }
        // Really shouldn't get to here, since this method will only be called if the randomized student is dirty
        else {
            return false;
        }
    }

    private boolean syncRandomizedStudentTrays(Token token, long batchId, String deviceId)
    {
        int dirtyRowCount = DbAdapter.countDirtyRowsForTable(DbAdapter.RANDOMIZED_STUDENT_TRAY_TABLE);
        int loopCount = 0;

        if (dirtyRowCount > 0) {

            do {
                RandomizedStudentTray rst = (RandomizedStudentTray) DbAdapter.getDirtyRowForTable(DbAdapter.RANDOMIZED_STUDENT_TRAY_TABLE);

                // If the return value is not null, go ahead and serialize it to JSON and send up to server.
                if (rst != null) {
                    try {
                        // Ignoring TEST HIGH SCHOOL and TEST ELEMENTARY SCHOOL's data
                        if (rst.getRandomizedStudentId() <= 0) {
                            loopCount++;
                            // Since we don't care about the TEST school's data, we are just going to mark
                            // them as cleaned.
                            DbAdapter.setRowClean(DbAdapter.RANDOMIZED_STUDENT_TRAY_TABLE, rst.getId());
                            continue;
                        }

                        // Convert randomized student tray to the randomized student tray staging for easier
                        // upload to server.
                        RandomizedStudentTrayStaging rsts = new RandomizedStudentTrayStaging(batchId, deviceId, rst);

                        // Convert to JSON
                        Gson gson = new GsonBuilder()
                                .registerTypeAdapter(Date.class, new DateDeserializer())
                                .create();

                        // Send it off to server
                        RequestBody requestBody = RequestBody.create(JSON_CONTENT_TYPE, gson.toJson(rsts));

                        Request request = new Request.Builder()
                                .url(RANDOMIZED_STUDENT_TRAY_STAGING_PATH)
                                .post(requestBody)
                                .header("Authorization", "Bearer " + token.getToken())
                                .build();

                        Response response = client.newCall(request).execute();

                        if (response.code() == 200) {
                            String responseJson = response.body().string();

                            RandomizedStudentTrayStaging returnedRsts = gson.fromJson(responseJson, RandomizedStudentTrayStaging.class);

                            // Verify the return of the server
                            if (rsts.compareTo(returnedRsts)) {
                                // If the return value is valid, go ahead and mark it as complete
                                DbAdapter.setRowClean(DbAdapter.RANDOMIZED_STUDENT_TRAY_TABLE, rst.getId());
                            } else {
                                // Print what went wrong and return false;
                                Log.e(TAG, "Error: Randomized student tray from tablet is not the same as the server's.");
                                return false;
                            }

                        } else {
                            // Print the error code from server and return false
                            Log.e(TAG, "Error while trying to push randomized student tray to server: " + response.message() + " " + response.body().string() + "\n" + response.toString());
                            return false;
                        }
                    } catch (Exception e) {
                        // Print the error message and return false
                        Log.e(TAG, "Error while trying to push randomized student tray to server: ", e);
                        return false;
                    }
                }

                loopCount++;

            } while (loopCount < dirtyRowCount);

            dirtyRowCount = DbAdapter.countDirtyRowsForTable(DbAdapter.RANDOMIZED_STUDENT_TRAY_TABLE);

            Log.i(TAG, "Dirty Row Count for " + DbAdapter.RANDOMIZED_STUDENT_TRAY_TABLE + " after pushing: " + dirtyRowCount);

            return dirtyRowCount == 0;
        }
        else {
            return true;
        }
    }
}
