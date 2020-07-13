package snhp.asu.edu.saladbar;

import android.app.Application;
import android.content.Context;

import snhp.asu.edu.saladbar.Models.Device;

public class SaladBarApp extends Application {

    private static final String TAG = "asu-snhp";

    private static Context mContext;

    public static DbAdapter mDbAdapter;

    public void onCreate(){
        super.onCreate();
        mContext = getApplicationContext();

        // Open the existing DB.  If no DB exists it will be created.
        mDbAdapter = new DbAdapter(this);

        Device device = new Device();
        DbAdapter.insertDevice(device);
    }

    public static Context getContext() {
        return mContext;
    }

    public static DbAdapter getDbAdapter() {
        return mDbAdapter;
    }

    @Override
    public void onTerminate() {

        mDbAdapter.close();

        super.onTerminate();

    }

}
