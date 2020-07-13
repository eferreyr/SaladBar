package snhp.asu.edu.saladbar.Activities;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import snhp.asu.edu.saladbar.BuildConfig;
import snhp.asu.edu.saladbar.DbAdapter;
import snhp.asu.edu.saladbar.Models.Device;
import snhp.asu.edu.saladbar.R;

public class AboutActivity extends AppCompatActivity {

    private TextView mAppVersionTextView;
    private TextView mDeviceGuidTextView;
    private Button  mOkButton;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_about);

        mAppVersionTextView = (TextView) findViewById(R.id.appVersionTextView);
        mDeviceGuidTextView = (TextView) findViewById(R.id.deviceGuid);
        mOkButton = (Button) findViewById(R.id.okButton);

        String versionNameStr = getString(R.string.app_version_info_prefix) + BuildConfig.VERSION_NAME;
        Device device = DbAdapter.getDevice();

        mAppVersionTextView.setText(versionNameStr);
        mDeviceGuidTextView.setText(device.getDeviceId());

        mOkButton.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                finish();
            }
        });
    }
}
