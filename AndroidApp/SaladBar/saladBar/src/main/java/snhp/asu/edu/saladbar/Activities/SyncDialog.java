package snhp.asu.edu.saladbar.Activities;

import android.content.Context;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.Window;
import android.widget.Button;
import android.widget.EditText;

import snhp.asu.edu.saladbar.DatabaseBackupTask;
import snhp.asu.edu.saladbar.DbAdapter;
import snhp.asu.edu.saladbar.R;
import snhp.asu.edu.saladbar.SyncTask;

public class SyncDialog extends AppCompatActivity {

    private EditText emailEditText;
    private EditText passwordEditText;
    private Button syncButton;
    private Button cancelButton;

    private Context ctx;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        supportRequestWindowFeature(Window.FEATURE_NO_TITLE);
        setContentView(R.layout.activity_sync_dialog);
        this.setFinishOnTouchOutside(false);
        ctx = this;

        emailEditText = (EditText) findViewById(R.id.emailEditText);
        passwordEditText = (EditText) findViewById(R.id.passwordEditText);
        syncButton = (Button) findViewById(R.id.syncButton);
        cancelButton = (Button) findViewById(R.id.cancelButton);

        syncButton.setOnClickListener(syncOnClickListner);
        cancelButton.setOnClickListener(cancelOnClickListner);
    }

    // Overriding onBackPressed to do nothing.
    @Override
    public void onBackPressed() {
    }

    private View.OnClickListener syncOnClickListner = new View.OnClickListener() {

        @Override
        public void onClick(View v)
        {
            // TODO: Don't allow sync if the backup failed, and tell the user why the sync failed.
            new DatabaseBackupTask(ctx, DatabaseBackupTask.SYNC).execute();
            new SyncTask(ctx, emailEditText.getText().toString(), passwordEditText.getText().toString()).execute();
        }
    };

    private View.OnClickListener cancelOnClickListner = new View.OnClickListener() {

        @Override
        public void onClick(View v)
        {
            // TODO: Temporary, need to remove this.
            boolean result = DbAdapter.temporarySetDirtyToNForNullAssent();
            Log.i("ASU","Result for setting dirty to N for null assent: " + result);
            finish();
        }
    };
}
