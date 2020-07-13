package snhp.asu.edu.saladbar;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.os.Environment;
import android.provider.Settings;
import android.util.Log;
import android.widget.Toast;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.nio.channels.FileChannel;
import java.text.SimpleDateFormat;
import java.util.Calendar;

import static android.content.ContentValues.TAG;

/**
 * Created by John on 9/29/17.
 */

public class DatabaseBackupTask extends AsyncTask<String, Void, Boolean> {

    public static final String AUTO = "auto-backup-";
    public static final String MANUAL = "manual-backup-";
    public static final String SYNC = "sync-backup-";
    public static final String UPGRADE = "database-upgrade-backup-";
    private final String OUTPUT_DIRECTORY_NAME = "Salad Bar Database Backups//";
    private SimpleDateFormat dateFormatter = new SimpleDateFormat("yyyy-MM-dd HHmmss");
    private SimpleDateFormat dateOnlyFormatter = new SimpleDateFormat("yyyy-MM-dd");

    private ProgressDialog mProgressSpinner;

    private Context ctx;
    private String backupType;
    private String outputInfo;
    private String deviceId;

    public DatabaseBackupTask(Context ctx, String backupType)
    {
        this.ctx = ctx;
        this.backupType = backupType;
        this.outputInfo = getOutputInfo();
        // TODO: Think about whether to grab it from settings or from the db.
        this.deviceId = Settings.Secure.getString(ctx.getContentResolver(), Settings.Secure.ANDROID_ID);
    }

    @Override
    protected void onPreExecute() {
        if(ctx != null) {
            mProgressSpinner = new ProgressDialog(ctx);
            mProgressSpinner.setMessage(outputInfo + "Backing up the database...");
            mProgressSpinner.setProgressStyle(ProgressDialog.STYLE_SPINNER);
            mProgressSpinner.show();
        }

        super.onPreExecute();
    }

    @Override
    protected Boolean doInBackground(String... params) {

        try
        {
            return exportDatabase();
        }
        catch (Exception e)
        {
            return false;
        }
    }

    @Override
    protected void onPostExecute(Boolean result) {

        try
        {
            if(ctx != null)
            {
                mProgressSpinner.dismiss();

                if(result)
                {
                    Toast.makeText(ctx, outputInfo + "Completed.", Toast.LENGTH_LONG).show();
                }

                else
                {
                    Toast.makeText(ctx, outputInfo + "Failed.", Toast.LENGTH_LONG).show();
                }
            }
        } catch (Exception e) {
            Log.e(TAG, "Weird error on dismiss of progress spinner.", e);
        }

        super.onPostExecute(result);
    }

    private String getOutputInfo()
    {
        switch (backupType)
        {
            case DatabaseBackupTask.AUTO:
                return "Auto Backup: ";
            case DatabaseBackupTask.MANUAL:
                return "Manual Backup: ";
            case DatabaseBackupTask.SYNC:
                return "Sync Backup: ";
            case DatabaseBackupTask.UPGRADE:
                return "Upgrade Backup: ";
            default:
                return "Error: ";
        }
    }

    // Exporting the db into SD card.
    private boolean exportDatabase() {

        String outputFileName = "";

        switch (backupType)
        {
            case DatabaseBackupTask.AUTO:
                outputFileName = backupType + DbAdapter.DATABASE_NAME + "-" + deviceId + "-" + dateOnlyFormatter.format(Calendar.getInstance().getTime()) + ".db";
                break;
            case DatabaseBackupTask.MANUAL:
                outputFileName = backupType + DbAdapter.DATABASE_NAME + "-" + deviceId + "-" + dateFormatter.format(Calendar.getInstance().getTime()) + ".db";
                break;
            case DatabaseBackupTask.SYNC:
                outputFileName = backupType + DbAdapter.DATABASE_NAME + "-" + deviceId + "-" + dateFormatter.format(Calendar.getInstance().getTime()) + ".db";
                break;
            case DatabaseBackupTask.UPGRADE:
                outputFileName = backupType + DbAdapter.DATABASE_NAME + "-" + deviceId + "-" + dateFormatter.format(Calendar.getInstance().getTime()) + ".db";
                break;
            default:
                outputFileName = "weird-error-" + DbAdapter.DATABASE_NAME + "-" + deviceId + "-" + dateFormatter.format(Calendar.getInstance().getTime()) + ".db";
                break;
        }

        try {
            File sd = Environment.getExternalStorageDirectory();
            File data = Environment.getDataDirectory();

            File outputDirectory = new File(sd, OUTPUT_DIRECTORY_NAME);

            if (sd.canWrite()) {
                if(!outputDirectory.exists())
                {
                    outputDirectory.mkdir();
                }

                String currentDBPath = "//data//snhp.asu.edu.saladbar//databases//" + DbAdapter.DATABASE_NAME;
                String backupDBPath = OUTPUT_DIRECTORY_NAME + outputFileName;
                File currentDB = new File(data, currentDBPath);
                File backupDB = new File(sd, backupDBPath);

                if (currentDB.exists()) {
                    FileChannel src = new FileInputStream(currentDB).getChannel();
                    FileChannel dst = new FileOutputStream(backupDB).getChannel();
                    dst.transferFrom(src, 0, src.size());
                    src.close();
                    dst.close();
                }

                return true;
            }

            return false;
        } catch (Exception e) {

        }

        return false;
    }
}
