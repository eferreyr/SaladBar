package snhp.asu.edu.saladbar.Activities;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.inputmethod.EditorInfo;
import android.view.inputmethod.InputMethodManager;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.TextView;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.util.ArrayList;

import snhp.asu.edu.saladbar.BuildConfig;
import snhp.asu.edu.saladbar.DbAdapter;
import snhp.asu.edu.saladbar.Models.Audit;
import snhp.asu.edu.saladbar.Models.InterventionDay;
import snhp.asu.edu.saladbar.Models.RandomizedStudent;
import snhp.asu.edu.saladbar.Models.RandomizedStudentTray;
import snhp.asu.edu.saladbar.Models.ResearchTeamMember;
import snhp.asu.edu.saladbar.Models.School;
import snhp.asu.edu.saladbar.Models.Tracking;
import snhp.asu.edu.saladbar.R;

public class TrayActivity extends AppCompatActivity {

    private static final String TAG = "asu-snhp";

    private EditText mTrayIdEditText;
    private Button mSaveButton;
    private Button mDoneButton;
    private TextView mStudentIdTextView;
    private ListView mTrayListView;

    private InterventionDay selectedInterventionDay;
    private School selectedSchool;
    private ResearchTeamMember selectedResearchTeamMember;
    private RandomizedStudent selectedRandomizedStudent;
    private ArrayList<Long> trays;
    private ArrayList<RandomizedStudentTray> randomizedStudentTrays;
    private ArrayAdapter trayListAdapter;
    private Activity activity;

    private Gson gson = new GsonBuilder().serializeNulls().setPrettyPrinting().setDateFormat("yyyy-MM-dd HH:mm:ss").create();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_tray);
        activity = this;

        trays = new ArrayList();
        randomizedStudentTrays = new ArrayList<>();

        selectedInterventionDay = (InterventionDay) this.getIntent().getExtras().get("selectedDay");
        selectedSchool = (School) this.getIntent().getExtras().get("selectedSchool");
        selectedRandomizedStudent = (RandomizedStudent) this.getIntent().getExtras().get("selectedRandomizedStudent");
        selectedResearchTeamMember = (ResearchTeamMember) this.getIntent().getExtras().get("selectedResearchTeamMember");

        randomizedStudentTrays = DbAdapter.getRandomizedStudentTraysForStudent(selectedRandomizedStudent.getId());
        copyRandomizedStudentTraysToTrays();

        mTrayIdEditText = (EditText) findViewById(R.id.trayIdEditText);
        mSaveButton = (Button) findViewById(R.id.saveButton);
        mDoneButton = (Button) findViewById(R.id.doneButton);
        mStudentIdTextView = (TextView) findViewById(R.id.studentIdTextView);
        mTrayListView = (ListView) findViewById(R.id.trayListView);

        mTrayIdEditText.setImeActionLabel("Add Tray", EditorInfo.IME_ACTION_DONE);
        mStudentIdTextView.setText("Student ID: " + selectedRandomizedStudent.getStudentId());

        trayListAdapter = new ArrayAdapter<>(this, android.R.layout.simple_list_item_1, trays);
        mTrayListView.setAdapter(trayListAdapter);

        if(trays.isEmpty())
        {
            mDoneButton.setEnabled(false);
        }

        mSaveButton.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {

                checkDate(v);
            }
        });

        mDoneButton.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {

                if(trays.size() >= 1)
                {
                    hideKeyboard(v);
                    activity.finish();
                }
            }
        });

        mTrayIdEditText.setOnEditorActionListener(new TextView.OnEditorActionListener() {
            @Override
            public boolean onEditorAction(TextView v, int actionId, KeyEvent event) {

                if((event != null) && (event.getKeyCode() == KeyEvent.KEYCODE_ENTER && event.getAction() == KeyEvent.ACTION_DOWN))
                {
                    checkDate(v);

                    return true;
                }

                else if(actionId == EditorInfo.IME_ACTION_DONE)
                {
                    checkDate(v);

                    return false;
                }
                return false;
            }
        });
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        menu.findItem(R.id.backupDb).setVisible(false);
        menu.findItem(R.id.sync).setVisible(false);
        menu.findItem(R.id.syncInfo).setVisible(false);
        menu.findItem(R.id.done).setVisible(false);
        menu.findItem(R.id.tracking).setVisible(false);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        String versionNameStr = getString(R.string.app_version_info_prefix) + BuildConfig.VERSION_NAME;
        switch(item.getItemId()) {
            case R.id.about:
                Intent aboutIntent = new Intent(activity, AboutActivity.class);
                startActivity(aboutIntent);
                return(true);
            default:
                return(super.onOptionsItemSelected(item));
        }
    }

    // Overriding onBackPressed to do nothing.
    @Override
    public void onBackPressed() {
    }

    // TODO: Need to change back
    private void checkDate(final View v)
    {
        saveTray(v);

//        android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(activity);
//        // Continue Button
//        builder.setPositiveButton("Continue", new DialogInterface.OnClickListener() {
//            public void onClick(DialogInterface dialog, int id) {
//                // User clicked continue
//                saveTray(v);
//            }
//        });
//        // Cancel Button
//        builder.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
//            public void onClick(DialogInterface dialog, int id) {
//                // User cancelled the dialog
//                dialog.dismiss();
//            }
//        });
//
//        // Create the AlertDialog
//        android.app.AlertDialog alertDialog = builder.create();
//        alertDialog.setTitle("Warning");
//        alertDialog.setMessage("You might have selected the wrong date, are you sure you want to continue?");
//
//        alertDialog.show();
    }

    private void saveTray(View v)
    {
        String currentIdEditText = mTrayIdEditText.getText().toString();
        addTracking(currentIdEditText);
        if(!currentIdEditText.isEmpty() && !currentIdEditText.equalsIgnoreCase(selectedRandomizedStudent.getStudentId()))
        {
            Long tray = new Long(0);

            try
            {
                tray = Long.parseLong(currentIdEditText);
            }
            catch (NumberFormatException e)
            {
                Log.d("", e.toString());
            }

            if(!trays.contains(tray))
            {
                RandomizedStudentTray rst = new RandomizedStudentTray(tray, selectedRandomizedStudent.getId(), selectedResearchTeamMember.getEmail());

                try {
                    DbAdapter.insertRandomizedStudentTray(rst);

                    String auditAfter = gson.toJson(rst);
                    Audit audit = new Audit(DbAdapter.RANDOMIZED_STUDENT_TRAY_TABLE, Audit.INSERT, null, auditAfter, selectedResearchTeamMember.getEmail(), null);
                    DbAdapter.insertAudit(audit);

                    trays.add(0, tray);
                    trayListAdapter.notifyDataSetChanged();
                }
                catch (Exception e) {
                    Log.e(TAG, "Error while trying to insert randomized student tray or insert audit", e);

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    // OK Button
                    builder.setPositiveButton("OK", null);
                    AlertDialog alertDialog = builder.create();
                    alertDialog.setTitle("Error");
                    alertDialog.setMessage("Error while trying to save tray. Error message: " + e.getMessage());

                    alertDialog.show();
                }
            }

            mTrayIdEditText.setText("");
        }

        if(!trays.isEmpty())
        {
            mDoneButton.setEnabled(true);
        }

        hideKeyboard(v);
    }

    // This is really bad I know, I hope this is temporary.
    private void copyRandomizedStudentTraysToTrays()
    {
        for(RandomizedStudentTray rst : randomizedStudentTrays)
        {
            trays.add(rst.getTrayId());
        }
    }

    private void hideKeyboard(View v)
    {
        InputMethodManager imm = (InputMethodManager) getSystemService(Context.INPUT_METHOD_SERVICE);
        imm.hideSoftInputFromWindow(v.getWindowToken(), 0);
    }

    private void addTracking(String input)
    {
        Tracking tracking = new Tracking(input, Tracking.TRAY, selectedSchool.getName(), selectedInterventionDay.getInterventionDate(), selectedResearchTeamMember.getEmail());
        DbAdapter.insertTracking(tracking);
    }
}
