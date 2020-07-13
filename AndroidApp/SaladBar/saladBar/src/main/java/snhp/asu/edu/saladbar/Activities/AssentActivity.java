package snhp.asu.edu.saladbar.Activities;

import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.text.method.ScrollingMovementMethod;
import android.view.KeyEvent;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.inputmethod.EditorInfo;
import android.view.inputmethod.InputMethodManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.text.SimpleDateFormat;
import java.util.ArrayList;

import snhp.asu.edu.saladbar.BuildConfig;
import snhp.asu.edu.saladbar.DatabaseBackupTask;
import snhp.asu.edu.saladbar.DbAdapter;
import snhp.asu.edu.saladbar.Models.Audit;
import snhp.asu.edu.saladbar.Models.InterventionDay;
import snhp.asu.edu.saladbar.Models.RandomizedStudent;
import snhp.asu.edu.saladbar.Models.ResearchTeamMember;
import snhp.asu.edu.saladbar.Models.School;
import snhp.asu.edu.saladbar.Models.Tracking;
import snhp.asu.edu.saladbar.R;

public class AssentActivity extends AppCompatActivity {

    private EditText mIdEditText;
    private TextView mInfoTextView;
    private TextView mSchoolNameTextView;
    private TextView mSchoolChantTextView;
    private Button mSearchButton;
    private Button mAssentButton;
    private Button mRefuseButton;
    private TextView mAssentCountTextView;
    private TextView mCurrentStudentIdTextView;
    private Button mSpanishButton;

    private Context ctx;
    private InterventionDay selectedInterventionDay;
    private School selectedSchool;
    private ResearchTeamMember selectedResearchTeamMember;
    private ArrayList<RandomizedStudent> randomizedStudents;
    private RandomizedStudent selectedRandomizedStudent;
    private String assentScript;
    private Boolean isEnglishScript;
    private int assentCount;
    private int selectedSchoolType;

    private String NORMAL_ASSENT_SCRIPT;
    private String ELEMENTARY_ASSENT_SCRIPT;
    private String ELEMENTARY_SPANISH_ASSENT_SCRIPT;

    private SimpleDateFormat dateFormatter = new SimpleDateFormat("yyyy-MM-dd");
    private Gson gson = new GsonBuilder().serializeNulls().setPrettyPrinting().setDateFormat("yyyy-MM-dd HH:mm:ss").create();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_assent);

        ctx = this;

        selectedInterventionDay = (InterventionDay) this.getIntent().getExtras().get("selectedDay");
        selectedSchool = (School) this.getIntent().getExtras().get("selectedSchool");
        selectedSchoolType = selectedSchool.getSchoolTypeId();
        selectedResearchTeamMember = (ResearchTeamMember) this.getIntent().getExtras().get("selectedResearchTeamMember");
        randomizedStudents = (ArrayList<RandomizedStudent>) this.getIntent().getExtras().get("randomizedStudents");

        mIdEditText = (EditText) findViewById(R.id.idEditText);
        mInfoTextView = (TextView) findViewById(R.id.infoTextView);
        mSchoolNameTextView = (TextView) findViewById(R.id.schoolNameTextView);
        mSchoolChantTextView = (TextView) findViewById(R.id.schoolChantTextView);
        mSearchButton = (Button) findViewById(R.id.searchButton);
        mAssentButton = (Button) findViewById(R.id.assentButton);
        mRefuseButton = (Button) findViewById(R.id.refuseButton);
        mAssentCountTextView = (TextView) findViewById(R.id.assentCountTextView);
        mCurrentStudentIdTextView = (TextView) findViewById(R.id.currentStudentIdTextView);
        mSpanishButton = (Button) findViewById(R.id.spanishButton);

        mIdEditText.requestFocus();

        isEnglishScript = true;
        createAssentScript();
        assentScript = selectedSchoolType == School.ELEMENTARY_SCHOOL ? ELEMENTARY_ASSENT_SCRIPT : NORMAL_ASSENT_SCRIPT;

        mCurrentStudentIdTextView.setText("Student ID:            ");

        mSchoolNameTextView.setText(selectedSchool.getName() + "\n" + dateFormatter.format(selectedInterventionDay.getInterventionDate()));

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

        mSchoolChantTextView.setText("Go "+ selectedSchool.getMascot() + "!");

        mInfoTextView.setMovementMethod(new ScrollingMovementMethod());
        mInfoTextView.setVisibility(View.INVISIBLE);
        mAssentButton.setVisibility(View.INVISIBLE);
        mRefuseButton.setVisibility(View.INVISIBLE);

        assentCount = DbAdapter.getAssentedCount(selectedSchool.getId(), selectedInterventionDay.getId());
        mAssentCountTextView.setText(assentCount + " Assented");

        mSearchButton.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {

                checkDate(v);
            }
        });

        mSpanishButton.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {

                isEnglishScript = !isEnglishScript;
                assentScript = isEnglishScript ? ELEMENTARY_ASSENT_SCRIPT : ELEMENTARY_SPANISH_ASSENT_SCRIPT;
                mInfoTextView.setText(assentScript);
                String buttonLabel = isEnglishScript ? "Spanish" : "English";
                mSpanishButton.setText(buttonLabel);
            }
        });

        mAssentButton.setOnClickListener(new View.OnClickListener() {
            public  void onClick(View v)
            {

                String auditBefore = gson.toJson(selectedRandomizedStudent);

                selectedRandomizedStudent.setAssent("Y");
                selectedRandomizedStudent.setModifiedBy(selectedResearchTeamMember.getEmail());
                DbAdapter.updateRandomizedStudent(selectedRandomizedStudent, "Y");

                String auditAfter = gson.toJson(selectedRandomizedStudent);
                Audit audit = new Audit(DbAdapter.RANDOMIZED_STUDENT_TABLE, Audit.UPDATE, auditBefore, auditAfter, selectedResearchTeamMember.getEmail(), null);
                DbAdapter.insertAudit(audit);

                assentCount = DbAdapter.getAssentedCount(selectedSchool.getId(), selectedInterventionDay.getId());
                mAssentCountTextView.setText(assentCount + " Assented");

                mCurrentStudentIdTextView.setText("");
                mIdEditText.setText("");
                mInfoTextView.setText("");
                mInfoTextView.setVisibility(View.INVISIBLE);
                mAssentButton.setVisibility(View.INVISIBLE);
                mRefuseButton.setVisibility(View.INVISIBLE);

                if(selectedSchoolType == School.ELEMENTARY_SCHOOL)
                {
                    mSpanishButton.setVisibility(View.GONE);
                }

                Intent trayIntent = new Intent(ctx, TrayActivity.class);

                trayIntent.putExtra("selectedDay", selectedInterventionDay);
                trayIntent.putExtra("selectedSchool", selectedSchool);
                trayIntent.putExtra("selectedResearchTeamMember", selectedResearchTeamMember);
                trayIntent.putExtra("selectedRandomizedStudent", selectedRandomizedStudent);

                startActivity(trayIntent);
            }
        });

        mRefuseButton.setOnClickListener(new View.OnClickListener() {
            public  void onClick(View v)
            {

                String auditBefore = gson.toJson(selectedRandomizedStudent);

                selectedRandomizedStudent.setAssent("N");
                selectedRandomizedStudent.setModifiedBy(selectedResearchTeamMember.getEmail());
                DbAdapter.updateRandomizedStudent(selectedRandomizedStudent, "Y");

                String auditAfter = gson.toJson(selectedRandomizedStudent);
                Audit audit = new Audit(DbAdapter.RANDOMIZED_STUDENT_TABLE, Audit.UPDATE, auditBefore, auditAfter, selectedResearchTeamMember.getEmail(), null);
                DbAdapter.insertAudit(audit);

                assentCount = DbAdapter.getAssentedCount(selectedSchool.getId(), selectedInterventionDay.getId());
                mAssentCountTextView.setText(assentCount + " Assented");

                mIdEditText.setText("");
                mInfoTextView.setText("");
                mInfoTextView.setVisibility(View.INVISIBLE);
                mAssentButton.setVisibility(View.INVISIBLE);
                mRefuseButton.setVisibility(View.INVISIBLE);

                if(selectedSchoolType == School.ELEMENTARY_SCHOOL)
                {
                    mSpanishButton.setVisibility(View.GONE);
                }
            }
        });

        mIdEditText.setOnEditorActionListener(new TextView.OnEditorActionListener() {
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
        menu.findItem(R.id.tracking).setVisible(false);
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
            case R.id.done:
                new DatabaseBackupTask(this, DatabaseBackupTask.AUTO).execute();
                finish();
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
        checkEligibility(v);

//        AlertDialog.Builder builder = new AlertDialog.Builder(ctx);
//        // Continue Button
//        builder.setPositiveButton("Continue", new DialogInterface.OnClickListener() {
//            public void onClick(DialogInterface dialog, int id) {
//                // User clicked continue
//                checkEligibility(v);
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
//        AlertDialog alertDialog = builder.create();
//        alertDialog.setTitle("Warning");
//        alertDialog.setMessage("You might have selected the wrong date, are you sure you want to continue?");
//
//        alertDialog.show();
    }

    private void checkEligibility(View v)
    {
        String studentId = mIdEditText.getText().toString();
        if(!studentId.isEmpty())
        {
            addTracking(studentId);

            mInfoTextView.setVisibility(View.INVISIBLE);
            mInfoTextView.setText("");


            mCurrentStudentIdTextView.setText("Student ID: " + studentId);
            mIdEditText.setText("");

            for(RandomizedStudent randomizedStudent : randomizedStudents)
            {
                if(randomizedStudent.getStudentId().equalsIgnoreCase(studentId))
                {
                    selectedRandomizedStudent = randomizedStudent;

                    mInfoTextView.setText(assentScript);
                    mInfoTextView.setBackgroundColor(Color.parseColor("#00b200"));
                    mInfoTextView.setVisibility(View.VISIBLE);
                    mAssentButton.setVisibility(View.VISIBLE);
                    mRefuseButton.setVisibility(View.VISIBLE);
                    if(selectedSchoolType == School.ELEMENTARY_SCHOOL)
                    {
                        mSpanishButton.setVisibility(View.VISIBLE);
                    }

                    hideKeyboard(v);

                    return;
                }
            }

            mInfoTextView.setVisibility(View.VISIBLE);
            mInfoTextView.setText("Sorry, you are not eligible!!\nThank you for your time");
            mInfoTextView.setBackgroundColor(Color.YELLOW);
            mAssentButton.setVisibility(View.INVISIBLE);
            mRefuseButton.setVisibility(View.INVISIBLE);

            if(selectedSchoolType == School.ELEMENTARY_SCHOOL)
            {
                mSpanishButton.setVisibility(View.GONE);
            }
        }

        hideKeyboard(v);
    }

    private void createAssentScript()
    {
        NORMAL_ASSENT_SCRIPT = "Hello! My name is " + selectedResearchTeamMember.getFirstName() +
                " and I work at Arizona State University. I am here for a research study" +
                "because we want to know more about students eating habits during lunch. " +
                "I'm asking students, like you, to allow me to weigh their lunch trays. " +
                "If you would like to participate you would need to go to the measurement " +
                "tables twice: once before you eat, and again after you have finished lunch "+
                "so we may weigh your lunch. It will take about 3 minutes and at the end we " +
                "will provide you with a small gift.";

        ELEMENTARY_ASSENT_SCRIPT = "Hi! My name is " + selectedResearchTeamMember.getFirstName() +
                " and I'm from Arizona State University in Phoenix. Can you tell me what grade you are in? " +
                "I'm visiting your school cafeteria today to see what kids eat for lunch. " +
                "I'm asking students like you to let me weigh and take a picture of their lunch tray. " +
                "If you're ok with this then you will need to go to have your lunch tray weighed and " +
                "photographed (point to weigh station) before you eat anything! After we weigh your tray " +
                "then you can sit down and eat your lunch. Nothing will happen to your lunch – your lunch " +
                "will be the same after we weigh and take a picture of it." +
                "\n\nWhen you are finished eating, you will need to give your tray to (point to tray depot) " +
                "and then we will give you a small gift. Please do not throw your tray away!!" +
                "\n\nYou do not have to do this if you don't want to and you can stop doing it at any time." +
                "\n\nWould it be ok with you for us to weigh and take a picture of your lunch tray?";

        ELEMENTARY_SPANISH_ASSENT_SCRIPT = "Hola mi nombre es " + selectedResearchTeamMember.getFirstName() +
                " y soy de Arizona State University en Phoenix. ¿Me puedes decir en cual grado estas? " +
                "Estoy visitando la cafetería de su escuela hoy para mirar que comen niños para lonche. " +
                "Estoy preguntando le a niños como tú que me dejen pesar y tomar una foto de su bandeja de comida. " +
                "¡Si estás bien con esto tienes que ir para que te pesen tu bandeja de comida y le tomen una foto " +
                "(punta a la estación de pesaje) antes que comas cualquier cosa! Después que te pesemos tu comida " +
                "te puedes ir a sentar y comer tu comida. Nada le ha pasar a tu comida – tu lonche va estar igual " +
                "después que lo pesemos and le tomamos una foto."+
                "\n\nCuando termines de comer, vas a llevar tu plato ha (punta al depósito de bandejas) y después te " +
                "vamos a dar un pequeño regarlo. ¡Por favor no tires tu bandeja!" +
                "\n\nNo tiene que hacer esto si no quiere y puede dejar de hacerlo en cualquier momento." +
                "\n\n¿Sería aceptable con usted para que pesemos y tomemos una imagen de su bandeja del almuerzo?";
    }

    private void hideKeyboard(View v)
    {
        InputMethodManager imm = (InputMethodManager) getSystemService(Context.INPUT_METHOD_SERVICE);
        imm.hideSoftInputFromWindow(v.getWindowToken(), 0);
        mIdEditText.requestFocus();
    }

    private void addTracking(String input)
    {
        Tracking tracking = new Tracking(input, Tracking.ASSENT, selectedSchool.getName(), selectedInterventionDay.getInterventionDate(), selectedResearchTeamMember.getEmail());
        DbAdapter.insertTracking(tracking);
    }
}
