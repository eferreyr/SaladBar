package snhp.asu.edu.saladbar.Activities;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import snhp.asu.edu.saladbar.DbAdapter;
import snhp.asu.edu.saladbar.R;

public class SyncInfoActivity extends AppCompatActivity {

    private TextView mUnsyncedRandomizedStudentCount;
    private TextView mUnsyncedRandomizedStudentTrayCount;
    private TextView mTotalRandomizedStudentCount;
    private TextView mTotalFutureRandomizedStudentCount;
    private Button  mOkButton;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_sync_info);

        mUnsyncedRandomizedStudentCount = (TextView) findViewById(R.id.unsyncedRandomizedStudentCount);
        mUnsyncedRandomizedStudentTrayCount = (TextView) findViewById(R.id.unsyncedRandomizedStudentTrayCount);
        mTotalRandomizedStudentCount = (TextView) findViewById(R.id.totalRandomizedStudentCount);
        mTotalFutureRandomizedStudentCount = (TextView) findViewById(R.id.totalFutureRandomizedStudentCount);
        mOkButton = (Button) findViewById(R.id.okButton);

        String unsyncedRandomizedStudentCountStr = Integer.toString(DbAdapter.countDirtyRowsForTable(DbAdapter.RANDOMIZED_STUDENT_TABLE));
        String unsyncedRandomizedStudentTrayCountStr = Integer.toString(DbAdapter.countDirtyRowsForTable(DbAdapter.RANDOMIZED_STUDENT_TRAY_TABLE));
        String totalRandomizedStudentCountStr =  Integer.toString(DbAdapter.getTotalRandomizedStudentCount());
        String totalFutureRandomizedStudentCountStr = Integer.toString(DbAdapter.getTotalFutureRandomizedStudentCount());

        mUnsyncedRandomizedStudentCount.setText(unsyncedRandomizedStudentCountStr);
        mUnsyncedRandomizedStudentTrayCount.setText(unsyncedRandomizedStudentTrayCountStr);
        mTotalRandomizedStudentCount.setText(totalRandomizedStudentCountStr);
        mTotalFutureRandomizedStudentCount.setText(totalFutureRandomizedStudentCountStr);

        // TODO: Show Audit's info as well and probably tracking as well.

        mOkButton.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                finish();
            }
        });
    }
}
