package snhp.asu.edu.saladbar.Activities;

import android.Manifest;
import android.app.Activity;
import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.Color;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Environment;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.io.File;
import java.io.FileOutputStream;
import java.io.FileWriter;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;

import snhp.asu.edu.saladbar.BuildConfig;
import snhp.asu.edu.saladbar.CustomSpinnerAdapter;
import snhp.asu.edu.saladbar.DatabaseBackupTask;
import snhp.asu.edu.saladbar.DbAdapter;
import snhp.asu.edu.saladbar.Models.InterventionDay;
import snhp.asu.edu.saladbar.Models.RandomizedStudent;
import snhp.asu.edu.saladbar.Models.ResearchTeamMember;
import snhp.asu.edu.saladbar.Models.School;
import snhp.asu.edu.saladbar.Models.Tracking;
import snhp.asu.edu.saladbar.R;

public class MainActivity extends AppCompatActivity {

    private TextView mSchoolNameTextView;
    private TextView mSchoolChantTextView;
    private Spinner mSchoolSpinner;
    private Spinner mInterventionDateSpinner;
    private Spinner mResearcherSpinner;
    private Button mNextButton;
    private ProgressDialog syncProgress;
    private Context ctx;
    private Activity activity;

    private ArrayAdapter<String> mListAdapter;
    private School selectedSchool;
    private InterventionDay selectedInterventionDay;
    private ResearchTeamMember selectedResearchTeamMember;
    private ArrayList<School> schools;
    private ArrayList<InterventionDay> interventionDays;
    private ArrayList<ResearchTeamMember> researchTeamMembers;
    private ArrayList<RandomizedStudent> randomizedStudents;

    private SimpleDateFormat dateFormatter = new SimpleDateFormat("yyyy-MM-dd");

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        ctx = this;
        activity = this;

        mSchoolNameTextView = (TextView) findViewById(R.id.schoolNameTextView);
        mSchoolChantTextView = (TextView) findViewById(R.id.schoolChantTextView);
        mSchoolSpinner = (Spinner) findViewById(R.id.schoolSpinner);
        mInterventionDateSpinner = (Spinner) findViewById(R.id.intervetionDateSpinner);
        mResearcherSpinner = (Spinner) findViewById(R.id.researcherSpinner);
        mNextButton = (Button) findViewById(R.id.nextButton);

        if(checkSelfPermission(Manifest.permission.WRITE_EXTERNAL_STORAGE) != PackageManager.PERMISSION_GRANTED)
        {
            requestPermissions(new String[]{Manifest.permission.WRITE_EXTERNAL_STORAGE}, 1);
        }

        schools = DbAdapter.getAllSchools();
        if(schools.size() == 0)
        {
            Toast.makeText(this, "Please sync first.", Toast.LENGTH_LONG).show();
        }

        schools.add(0, new School(0, "", "", "", "", new byte[1], 1, ""));

        CustomSpinnerAdapter schoolAdapter = new CustomSpinnerAdapter(
                ctx, schools);
        mSchoolSpinner.setAdapter(schoolAdapter);

        // OnItemSelectedListener for the school spinner, when a school is selected, the corresponding
        // intervention days will be populated.
        mSchoolSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parentView, View selectedItemView, int position, long id) {
                selectedSchool = schools.get(position);
                interventionDays = DbAdapter.getAllInterventionDaysForSchool(selectedSchool.getId());
                interventionDays.add(0, new InterventionDay(0, 0, Calendar.getInstance().getTime(), "N", "Y"));

                CustomSpinnerAdapter interventionDayAdapter = new CustomSpinnerAdapter(
                        ctx, interventionDays);
                mInterventionDateSpinner.setAdapter(interventionDayAdapter);

                if(position == 0)
                {
                    mSchoolNameTextView.setText(R.string.school_name_default);
                    mSchoolNameTextView.setBackgroundColor(Color.TRANSPARENT);
                    mSchoolChantTextView.setText("");
                }

                else
                {
                    mSchoolNameTextView.setText(selectedSchool.getName());
                    try
                    {
                        mSchoolNameTextView.setBackgroundColor(Color.parseColor(selectedSchool.getFirstColor()));
                    }
                    catch (IllegalArgumentException e)
                    {
                        mSchoolNameTextView.setBackgroundColor(Color.TRANSPARENT);
                    }

                    // TODO: Make this better
                    if (selectedSchool.getFirstColor().equalsIgnoreCase("black")) {
                        mSchoolNameTextView.setTextColor(Color.WHITE);
                    }
                    else {
                        mSchoolNameTextView.setTextColor(Color.BLACK);
                    }


                    mSchoolChantTextView.setText("Go " + selectedSchool.getMascot() + "!");
                }

            }

            @Override
            public void onNothingSelected(AdapterView<?> parentView) {
                // your code here
            }
        });

        mInterventionDateSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parentView, View selectedItemView, int position, long id) {
                selectedInterventionDay = interventionDays.get(position);

                if(position == 0)
                {
                    if(selectedSchool.getName().isEmpty())
                    {
                        mSchoolNameTextView.setText(R.string.school_name_default);
                    }
                    else
                    {
                        mSchoolNameTextView.setText(selectedSchool.getName());
                    }
                }

                else
                {
                    mSchoolNameTextView.setText(selectedSchool.getName() + "\n" + dateFormatter.format(selectedInterventionDay.getInterventionDate()));
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parentView) {
                // your code here
            }
        });

        researchTeamMembers = DbAdapter.getAllResearchTeamMembers();
        researchTeamMembers.add(0, new ResearchTeamMember(0, "", "", "" , ""));

        CustomSpinnerAdapter researcherAdapter = new CustomSpinnerAdapter(
                ctx, researchTeamMembers);
        mResearcherSpinner.setAdapter(researcherAdapter);

        mNextButton.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {

                if(mSchoolSpinner.getSelectedItemPosition() == 0
                        || mInterventionDateSpinner.getSelectedItemPosition() == 0
                        || mResearcherSpinner.getSelectedItemPosition() == 0)
                {
                    AlertDialog alertDialog = new AlertDialog.Builder(ctx).create();
                    alertDialog.setTitle("Information Recorded");
                    alertDialog.setMessage("Please make sure you have selected all.");
                    alertDialog.setButton(AlertDialog.BUTTON_NEUTRAL, "OK",
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int which) {
                                    dialog.dismiss();
                                }
                            });
                    alertDialog.show();
                }

                else
                {
                    String todaysDate = dateFormatter.format(Calendar.getInstance().getTime());
                    String selectedDate = dateFormatter.format(selectedInterventionDay.getInterventionDate());

                    if(todaysDate.equals(selectedDate))
                    {
                        selectedResearchTeamMember = researchTeamMembers.get(mResearcherSpinner.getSelectedItemPosition());
                        randomizedStudents = DbAdapter.getRandomizedStudentsForSchoolAndInterventionDays(selectedSchool.getId(), selectedInterventionDay.getId());

                        Intent assentIntent = new Intent(ctx, AssentActivity.class);

                        assentIntent.putExtra("selectedDay", selectedInterventionDay);
                        assentIntent.putExtra("selectedSchool", selectedSchool);
                        assentIntent.putExtra("selectedResearchTeamMember", selectedResearchTeamMember);
                        assentIntent.putExtra("randomizedStudents", randomizedStudents);

                        startActivity(assentIntent);
                    }

                    else
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                        // Continue Button
                        builder.setPositiveButton("Continue", new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int id) {
                                // User clicked continue
                                selectedResearchTeamMember = researchTeamMembers.get(mResearcherSpinner.getSelectedItemPosition());
                                randomizedStudents = DbAdapter.getRandomizedStudentsForSchoolAndInterventionDays(selectedSchool.getId(), selectedInterventionDay.getId());

                                Intent assentIntent = new Intent(ctx, AssentActivity.class);

                                assentIntent.putExtra("selectedDay", selectedInterventionDay);
                                assentIntent.putExtra("selectedSchool", selectedSchool);
                                assentIntent.putExtra("selectedResearchTeamMember", selectedResearchTeamMember);
                                assentIntent.putExtra("randomizedStudents", randomizedStudents);

                                startActivity(assentIntent);
                            }
                        });
                        // Cancel Button
                        builder.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int id) {
                                // User cancelled the dialog
                                dialog.dismiss();
                            }
                        });

                        // Create the AlertDialog
                        AlertDialog alertDialog = builder.create();
                        alertDialog.setTitle("Warning");
                        alertDialog.setMessage("You might have selected the wrong date, are you sure you want to continue?");

                        alertDialog.show();
                    }
                }
            }
        });
    }

    @Override
    protected void onResume() {
        super.onResume();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        menu.findItem(R.id.done).setVisible(false);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        String versionNameStr = getString(R.string.app_version_info_prefix) + BuildConfig.VERSION_NAME;
        switch(item.getItemId()) {
            case R.id.about:
                Intent aboutIntent = new Intent(ctx, AboutActivity.class);
                startActivity(aboutIntent);
                return(true);
            case R.id.backupDb:
                new DatabaseBackupTask(ctx, DatabaseBackupTask.MANUAL).execute();
                return(true);
            case R.id.sync:
                if (isNetworkConnected()) {
                    Intent syncIntent = new Intent(ctx, SyncDialog.class);
                    startActivity(syncIntent);
                    // TODO: On first sync after the initial install of the app, update the schools list
                    // and intervention day list (if applicable) after the sync finishes.
                }
                else {
                    Toast.makeText(this, "Wifi is not connected. Please check connection.", Toast.LENGTH_LONG).show();
                }

                return(true);
            case R.id.syncInfo:
                Intent syncInfoIntent = new Intent(ctx, SyncInfoActivity.class);
                startActivity(syncInfoIntent);
                return(true);
            case R.id.tracking:
                new OutputTracking(ctx, DbAdapter.getAllTrackings()).execute();
                return(true);
            default:
                return(super.onOptionsItemSelected(item));
        }
    }

    private boolean isNetworkConnected() {
        ConnectivityManager connMgr = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo networkInfo = connMgr.getActiveNetworkInfo();
        return networkInfo != null && networkInfo.isConnected();
    }


    private static class OutputTracking extends AsyncTask<String, Void, Boolean> {

        private ProgressDialog mProgressSpinner = null;
        private Gson gson = new GsonBuilder().serializeNulls().setPrettyPrinting().setDateFormat("yyyy-MM-dd HH:mm:ss").create();
        private SimpleDateFormat dateFormatter = new SimpleDateFormat("yyyy-MM-dd HHmmss");

        private Context ctx;
        private List<Tracking> trackings;

        public OutputTracking(Context ctx, List<Tracking> trackings) {
            this.ctx = ctx;
            this.trackings = trackings;
        }

        @Override
        protected void onPreExecute() {
            mProgressSpinner = new ProgressDialog(ctx);
            mProgressSpinner.setMessage("Outputing tracking...");
            mProgressSpinner.setProgressStyle(ProgressDialog.STYLE_SPINNER);
            mProgressSpinner.show();
            super.onPreExecute();
        }

        @Override
        protected Boolean doInBackground(String... params) {
            boolean resultVal = false;

            try {
                String outputString = gson.toJson(trackings);
                String filename = "TrackingOutput" + "-" + dateFormatter.format(Calendar.getInstance().getTime()) + ".json";
                FileOutputStream outputStream;

                File sd = Environment.getExternalStorageDirectory();

                File outputDirectory = new File(sd.getAbsolutePath());

                if (sd.canWrite()) {
                    FileWriter out = new FileWriter(new File(sd, filename));
                    out.write(outputString);
                    out.close();

                    return true;
                }

                return false;
            } catch (Exception e) {
                Log.d("Error", "Error outputting tracking..." + e.toString());
            }
            return resultVal;
        }

        @Override
        protected void onPostExecute(Boolean result) {

            mProgressSpinner.dismiss();

            if(result)
            {
                Toast.makeText(ctx, "Output Completed.", Toast.LENGTH_SHORT).show();
            }

            else
            {
                Toast.makeText(ctx, "Output Failed.", Toast.LENGTH_SHORT).show();
            }

            mProgressSpinner = null;
            super.onPostExecute(result);
        }
    }

//    private static class testSchoolMascot extends AsyncTask<String, Void, Integer> {
//
//        private ProgressDialog mProgressSpinner = null;
//        private Context ctx;
//        private Activity v;
//        private Bitmap bmp;
//
//        public testSchoolMascot(Context ctx, Activity v) {
//            this.ctx = ctx;
//            this.v = v;
//        }
//
//        @Override
//        protected void onPreExecute() {
//            mProgressSpinner = new ProgressDialog(ctx);
//            mProgressSpinner.setMessage("Copying the DB...");
//            mProgressSpinner.setProgressStyle(ProgressDialog.STYLE_SPINNER);
//            mProgressSpinner.show();
//            super.onPreExecute();
//        }
//
//        @Override
//        protected Integer doInBackground(String... params) {
//            int resultVal = 0;
//            try {
//                Drawable drawable = ctx.getResources().getDrawable(R.mipmap.ic_launcher, ctx.getTheme());
//                Bitmap bitmap = ((BitmapDrawable)drawable).getBitmap();
//                ByteArrayOutputStream stream = new ByteArrayOutputStream();
//                bitmap.compress(Bitmap.CompressFormat.PNG, 100, stream);
//                byte[] mascot = stream.toByteArray();
//
//                // Temp
//                School s = new School(1, "Chaparral High School", "HS", "red", mascot);
//
//                DbAdapter.insertSchool(s);
//
//                ArrayList<School> sArr = DbAdapter.getAllSchools();
//
//                bmp = BitmapFactory.decodeByteArray(sArr.get(0).getMascot(), 0, sArr.get(0).getMascot().length);
//
//                resultVal = 1;
//            } catch (Exception e) {
//                Log.e("abc", "Failure copying DB", e);
//            }
//            return resultVal;
//        }
//
//        @Override
//        protected void onPostExecute(Integer result) {
//            try {
//                mProgressSpinner.dismiss();
//            } catch (Exception e) {
//                Log.e("ab", "Weird error on dismiss of progress spinner.", e);
//            }
//
//            mProgressSpinner = null;
//            super.onPostExecute(result);
//        }
//    }
}
