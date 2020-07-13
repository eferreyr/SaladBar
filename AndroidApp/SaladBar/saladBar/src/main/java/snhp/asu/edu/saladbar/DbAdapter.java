package snhp.asu.edu.saladbar;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.SQLException;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.database.sqlite.SQLiteStatement;
import android.util.Log;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;

import snhp.asu.edu.saladbar.Models.Audit;
import snhp.asu.edu.saladbar.Models.Device;
import snhp.asu.edu.saladbar.Models.InterventionDay;
import snhp.asu.edu.saladbar.Models.RandomizedStudent;
import snhp.asu.edu.saladbar.Models.RandomizedStudentTray;
import snhp.asu.edu.saladbar.Models.ResearchTeamMember;
import snhp.asu.edu.saladbar.Models.School;
import snhp.asu.edu.saladbar.Models.Token;
import snhp.asu.edu.saladbar.Models.Tracking;

import static android.content.ContentValues.TAG;

/**
 * Created by John on 8/6/17.
 */

public class DbAdapter {

    public static final String DATABASE_NAME = "saladbardb";
    private static SimpleDateFormat dateFormatter = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
    private static SimpleDateFormat dateOnlyFormatter = new SimpleDateFormat("yyyy-MM-dd");

    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //
    // VERY IMPORTANT!!!  The days of being able to drop and recreate the entire
    // DB ARE OVER!  If you change the DB version you MUST address the migration
    // in the method "onUpgrade()" below.

    private static final int DATABASE_VERSION = 1;
    private static final int LAST_UNMIGRATED_VERSION = 0;  // Zero means migrations not required.

    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    private final DatabaseHelper mDbHelper;
    private static SQLiteDatabase mDb;

    // Table names.
    public static final String SCHOOL_TABLE = "Schools";
    public static final String INTERVENTION_DAY_TABLE = "InterventionDays";
    public static final String RANDOMIZED_STUDENT_TABLE = "RandomizedStudents";
    public static final String RANDOMIZED_STUDENT_TRAY_TABLE = "RandomizedStudentTrays";
    public static final String RESEARCH_TEAM_MEMBER_TABLE = "ResearchTeamMembers";
    public static final String DEVICE_TABLE = "Devices";
    public static final String AUDIT_TABLE = "Audits";
    public static final String TOKEN_TABLE = "Tokens";
    public static final String TRACKING_TABLE = "Trackings";

    // Common row ID.
    public static final String KEY_ROW_ID = "id";
    // TODO: Change this back to -1
    public static final long INVALID_ID = -5;

    public static final String KEY_ACTIVE = "active";

    // Common "dirty" column and index.
    public static final String KEY_DIRTY = "dirty";
    public static final String DIRTY_INDEX = "CREATE INDEX %t_dirty_index ON %t (" + KEY_DIRTY + ");";
    private static String dirtyIndex(String tableName) {
        return DIRTY_INDEX.replace("%t", tableName);
    }

    // Common audit columns and trigger.
    public final static String KEY_CREATE_BY = "created_by";
    public final static String KEY_UPDATE_BY = "modified_by";
    public final static String KEY_CREATE_TIMESTAMP = "dt_created";
    public final static String KEY_UPDATE_TIMESTAMP = "dt_modified";
    public final static String UPDATE_TIMESTAMP_TRIGGER =
            "CREATE TRIGGER %t_update_ts_trigger AFTER UPDATE ON %t" +
                    " BEGIN" +
                    " UPDATE %t SET dt_modified = CURRENT_TIMESTAMP" +
                    " WHERE " + KEY_ROW_ID + " = old." + KEY_ROW_ID + ";" +
                    " END";
    private static String createUpdateTimestampTrigger(String tableName) {
        return UPDATE_TIMESTAMP_TRIGGER.replace("%t", tableName);
    }



    // -----------------------------------------------------
    // School table.
    // This table contains the metadata for all schools.
    // -----------------------------------------------------
    public static final String KEY_SCHOOL_ID = "school_id";
    public static final String KEY_SCHOOL_NAME = "name";
    public static final String KEY_DISTRICT = "district";
    public static final String KEY_MASCOT = "mascot";
    public static final String KEY_COLORS = "colors";
    public static final String KEY_SCHOOL_LOGO = "school_logo";
    public static final String KEY_SCHOOL_TYPE_ID = "school_type_id";

    private static final String SCHOOL_COLUMNS = " (" +
            KEY_ROW_ID                   + " INTEGER PRIMARY KEY, " +
            KEY_SCHOOL_NAME              + " TEXT NOT NULL, " +
            KEY_DISTRICT                 + " TEXT NULL, " +
            KEY_MASCOT                   + " TEXT NULL, " +
            KEY_COLORS                   + " TEXT NULL, " +
            KEY_SCHOOL_LOGO              + " BLOB NULL, " +
            KEY_SCHOOL_TYPE_ID           + " INTEGER NOT NULL, " +
            KEY_ACTIVE                   + " TEXT NOT NULL DEFAULT 'Y', " +

            KEY_CREATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_CREATE_BY                + " TEXT NOT NULL DEFAULT 'SYSTEM', " +
            KEY_UPDATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_UPDATE_BY                + " TEXT NULL);";
    private static final String DATABASE_CREATE_SCHOOL =
            "CREATE TABLE " + SCHOOL_TABLE + SCHOOL_COLUMNS;


    // -----------------------------------------------------
    // Intervention day table.
    // This table contains the intervention day for each schools
    // -----------------------------------------------------
    public static final String KEY_INTERVENTION_DATE = "dt_intervention";
    public static final String KEY_INTERVENTION_FINISHED = "intervention_finished";

    private static final String INTERVENTION_DAY_COLUMNS = " (" +
            KEY_ROW_ID                   + " INTEGER PRIMARY KEY, " +
            KEY_SCHOOL_ID                + " INTEGER NOT NULL, " +
            KEY_INTERVENTION_DATE        + " TIMESTAMP NOT NULL, " +
            KEY_INTERVENTION_FINISHED    + " TEXT NOT NULL DEFAULT 'N', " +
            KEY_ACTIVE                   + " TEXT NOT NULL DEFAULT 'Y', " +
            
            KEY_CREATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_CREATE_BY                + " TEXT NOT NULL DEFAULT 'SYSTEM', " +
            KEY_UPDATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_UPDATE_BY                + " TEXT NULL);";
    private static final String DATABASE_CREATE_INTERVENTION_DATE =
            "CREATE TABLE " + INTERVENTION_DAY_TABLE + INTERVENTION_DAY_COLUMNS;


    // -----------------------------------------------------
    // Randomized student table.
    // This table contains the students that were randomized
    // to be selected for the study.
    // -----------------------------------------------------
    public static final String KEY_STUDENT_ID = "student_id";
    public static final String KEY_INTERVENTION_DAY_ID = "intervention_day_id";
    public static final String KEY_GENDER = "gender";
    public static final String KEY_GRADE = "grade";
    public static final String KEY_ASSENT = "assent";


    private static final String RANDOMIZED_STUDENT_COLUMNS = " (" +
            KEY_ROW_ID                   + " INTEGER PRIMARY KEY, " +
            KEY_SCHOOL_ID                + " INTEGER NOT NULL, " +
            KEY_INTERVENTION_DAY_ID      + " INTEGER NOT NULL, " +
            KEY_STUDENT_ID               + " TEXT NOT NULL, " +
            KEY_GENDER                   + " TEXT NULL, " +
            KEY_GRADE                    + " TEXT NULL, " +
            KEY_ASSENT                   + " TEXT NULL, " + // 'Y' or 'N'
            KEY_DIRTY                    + " TEXT NOT NULL DEFAULT 'Y', " + // 'Y' or 'N'

            KEY_CREATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_CREATE_BY                + " TEXT NOT NULL DEFAULT 'SYSTEM', " +
            KEY_UPDATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_UPDATE_BY                + " TEXT NULL);";
    private static final String DATABASE_CREATE_RANDOMIZED_STUDENT =
            "CREATE TABLE " + RANDOMIZED_STUDENT_TABLE + RANDOMIZED_STUDENT_COLUMNS;


    // -----------------------------------------------------
    // Randomized Student Tray table.
    // This table contains the trays for each randomized student
    // -----------------------------------------------------
    public static final String KEY_TRAY_ID = "tray_id";
    public static final String KEY_RANDOMIZED_STUDENT_ID = "randomized_student_id"; // This is the row ID of Randomized Student

    private static final String RANDOMIZED_STUDENT_TRAY_COLUMNS = " (" +
            KEY_ROW_ID                   + " INTEGER PRIMARY KEY, " +
            KEY_TRAY_ID                  + " INTEGER NOT NULL, " +
            KEY_RANDOMIZED_STUDENT_ID    + " INTEGER NOT NULL, " +  // This is the row ID of Randomized Student
            KEY_DIRTY                    + " TEXT NOT NULL DEFAULT 'Y', " + // 'Y' or 'N'

            KEY_CREATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_CREATE_BY                + " TEXT NOT NULL DEFAULT 'SYSTEM', " +
            KEY_UPDATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_UPDATE_BY                + " TEXT NULL);";
    private static final String DATABASE_CREATE_RANDOMIZED_STUDENT_TRAY =
            "CREATE TABLE " + RANDOMIZED_STUDENT_TRAY_TABLE + RANDOMIZED_STUDENT_TRAY_COLUMNS;


    // -----------------------------------------------------
    // Research Team Member table.
    // This table contains all research team members.
    // -----------------------------------------------------
    public static final String KEY_RESEARCHER_FIRST_NAME = "first_name";
    public static final String KEY_RESEARCHER_LAST_NAME = "last_name";
    public static final String KEY_RESEARCHER_EMAIL = "email";

    private static final String RESEARCHER_COLUMNS = " (" +
            KEY_ROW_ID                   + " INTEGER PRIMARY KEY, " +
            KEY_RESEARCHER_EMAIL         + " TEXT NOT NULL, " +
            KEY_RESEARCHER_FIRST_NAME    + " TEXT NOT NULL, " +
            KEY_RESEARCHER_LAST_NAME     + " TEXT NOT NULL, " +
            KEY_ACTIVE                   + " TEXT NOT NULL DEFAULT 'N', " + // 'Y' or 'N'

            KEY_CREATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_CREATE_BY                + " TEXT NOT NULL DEFAULT 'SYSTEM', " +
            KEY_UPDATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_UPDATE_BY                + " TEXT NULL);";
    private static final String DATABASE_CREATE_RESEARCHER =
            "CREATE TABLE " + RESEARCH_TEAM_MEMBER_TABLE + RESEARCHER_COLUMNS;


    // -----------------------------------------------------
    // Randomized Student Tray table.
    // This table contains the trays for each randomized student
    // -----------------------------------------------------
    public static final String KEY_DEVICE_ID = "device_id";
    public static final String KEY_DEVICE_NAME = "device_name";

    private static final String DEVICE_COLUMNS = " (" +
            KEY_ROW_ID                   + " INTEGER PRIMARY KEY, " +
            KEY_DEVICE_ID                + " TEXT NOT NULL, " +
            KEY_DEVICE_NAME              + " TEXT NOT NULL, " +  // This is the row ID of Randomized Student
            KEY_DIRTY                    + " TEXT NOT NULL DEFAULT 'Y', " + // 'Y' or 'N'

            KEY_CREATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_CREATE_BY                + " TEXT NOT NULL DEFAULT 'SYSTEM', " +
            KEY_UPDATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_UPDATE_BY                + " TEXT NULL);";
    private static final String DATABASE_CREATE_DEVICE =
            "CREATE TABLE " + DEVICE_TABLE + DEVICE_COLUMNS;


    // -----------------------------------------------------
    // Audit table.
    // This table contains the audits
    // -----------------------------------------------------
    public static final String KEY_TABLE_NAME = "table_name";
    public static final String KEY_ACTION = "action";
    public static final String KEY_VALUE_BEFORE = "values_before";
    public static final String KEY_VALUE_AFTER = "values_after";

    private static final String AUDIT_COLUMNS = " (" +
            KEY_ROW_ID                   + " INTEGER PRIMARY KEY, " + // need to seed ids for each app (100,000, 200,000, etc.)
            KEY_TABLE_NAME               + " TEXT NOT NULL, " +
            KEY_ACTION                   + " TEXT NULL, " +
            KEY_VALUE_BEFORE             + " TEXT NULL, " + // JSON value
            KEY_VALUE_AFTER              + " TEXT NULL, " + // JSON value
            KEY_DIRTY                    + " TEXT NOT NULL DEFAULT 'Y', " + // 'Y' or 'N'

            KEY_CREATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_CREATE_BY                + " TEXT NOT NULL DEFAULT 'SYSTEM', " +
            KEY_UPDATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_UPDATE_BY                + " TEXT NULL);";
    private static final String DATABASE_CREATE_AUDIT =
            "CREATE TABLE " + AUDIT_TABLE + AUDIT_COLUMNS;

    // -----------------------------------------------------
    // Token table.
    // This table contains the tokens that the server return
    // -----------------------------------------------------
    public static final String KEY_TOKEN = "token";
    public static final String KEY_TOKEN_EXPIRATION_TIME = "token_expiration_time";

    private static final String TOKEN_COLUMNS = " (" +
            KEY_ROW_ID                   + " INTEGER PRIMARY KEY, " +
            KEY_RESEARCHER_EMAIL         + " TEXT NOT NULL, " +
            KEY_TOKEN                    + " TEXT NOT NULL, " +
            KEY_TOKEN_EXPIRATION_TIME    + " TIMESTAMP NOT NULL, " +

            KEY_CREATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_CREATE_BY                + " TEXT NOT NULL DEFAULT 'SYSTEM', " +
            KEY_UPDATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_UPDATE_BY                + " TEXT NULL);";
    private static final String DATABASE_CREATE_TOKEN =
            "CREATE TABLE " + TOKEN_TABLE + TOKEN_COLUMNS;

    // -----------------------------------------------------
    // Tracking table.
    // This table contains the tracking information
    // -----------------------------------------------------
    public static final String KEY_INPUT = "input";
    public static final String KEY_LOCATION = "location";

    private static final String TRACKING_COLUMNS = " (" +
            KEY_ROW_ID                   + " INTEGER PRIMARY KEY, " +
            KEY_INPUT                    + " TEXT NOT NULL, " +
            KEY_LOCATION                 + " TEXT NOT NULL, " +
            KEY_SCHOOL_NAME              + " TEXT NOT NULL, " +
            KEY_INTERVENTION_DATE        + " TEXT NOT NULL, " +

            KEY_CREATE_TIMESTAMP         + " TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
            KEY_CREATE_BY                + " TEXT NOT NULL DEFAULT 'SYSTEM');";
    private static final String DATABASE_CREATE_TRACKING =
            "CREATE TABLE " + TRACKING_TABLE + TRACKING_COLUMNS;


    public DbAdapter(Context ctx) {

        // Open the DB. Will create an empty one if it doesn't exist or
        // if this version of the application requires a different DB format.

        mDbHelper = new DatabaseHelper(ctx);
        mDb = mDbHelper.getWritableDatabase();
    }


    public void close() {
        mDbHelper.close();
    }

    private static class DatabaseHelper extends SQLiteOpenHelper {

        DatabaseHelper(Context context) {
            super(context, DATABASE_NAME, null, DATABASE_VERSION);
        }

        @Override
        public void onCreate(SQLiteDatabase db) {

            db.execSQL(DATABASE_CREATE_SCHOOL);
            db.execSQL(createUpdateTimestampTrigger(SCHOOL_TABLE));

            db.execSQL(DATABASE_CREATE_INTERVENTION_DATE);
            db.execSQL(createUpdateTimestampTrigger(INTERVENTION_DAY_TABLE));

            db.execSQL(DATABASE_CREATE_RANDOMIZED_STUDENT);
            db.execSQL(dirtyIndex(RANDOMIZED_STUDENT_TABLE));
            db.execSQL(createUpdateTimestampTrigger(RANDOMIZED_STUDENT_TABLE));

            db.execSQL(DATABASE_CREATE_RANDOMIZED_STUDENT_TRAY);
            db.execSQL(dirtyIndex(RANDOMIZED_STUDENT_TRAY_TABLE));
            db.execSQL(createUpdateTimestampTrigger(RANDOMIZED_STUDENT_TRAY_TABLE));

            db.execSQL(DATABASE_CREATE_RESEARCHER);
            db.execSQL(createUpdateTimestampTrigger(RESEARCH_TEAM_MEMBER_TABLE));

            db.execSQL(DATABASE_CREATE_DEVICE);
            db.execSQL(dirtyIndex(DEVICE_TABLE));
            db.execSQL(createUpdateTimestampTrigger(DEVICE_TABLE));

            db.execSQL(DATABASE_CREATE_AUDIT);
            db.execSQL(dirtyIndex(AUDIT_TABLE));
            db.execSQL(createUpdateTimestampTrigger(AUDIT_TABLE));

            db.execSQL(DATABASE_CREATE_TOKEN);
            db.execSQL(createUpdateTimestampTrigger(TOKEN_TABLE));

            db.execSQL(DATABASE_CREATE_TRACKING);
            db.execSQL(createUpdateTimestampTrigger(TRACKING_TABLE));
        }

        @Override
        public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
            // Backing up the db before dropping.
            new DatabaseBackupTask(null, DatabaseBackupTask.UPGRADE).execute();

            // Drop and recreate all tables.
            db.execSQL("DROP TABLE IF EXISTS " + SCHOOL_TABLE);
            db.execSQL("DROP TABLE IF EXISTS " + INTERVENTION_DAY_TABLE);
            db.execSQL("DROP TABLE IF EXISTS " + RANDOMIZED_STUDENT_TABLE);
            db.execSQL("DROP TABLE IF EXISTS " + RANDOMIZED_STUDENT_TRAY_TABLE);
            db.execSQL("DROP TABLE IF EXISTS " + RESEARCH_TEAM_MEMBER_TABLE);
            db.execSQL("DROP TABLE IF EXISTS " + DEVICE_TABLE);
            db.execSQL("DROP TABLE IF EXISTS " + AUDIT_TABLE);
            db.execSQL("DROP TABLE IF EXISTS " + TOKEN_TABLE);
            db.execSQL("DROP TABLE IF EXISTS " + TRACKING_TABLE);
            onCreate(db);
        }
    }

    // -------------------------------------------------------------------------------------
    // ------------------- Dirty record processing -----------------------------------------
    // -------------------------------------------------------------------------------------

    // The basic strategy is for the "dirty" flag in each "model object" to be set to '1'
    // whenever that record is inserted or updated. The upload to the host takes
    // place in three steps.
    //
    // Step 1: Mark a set of records as being "in the laundry" (=2). This allows other
    // processes to continue to make updates since that update would simply set the flag
    // back to dirty. See step 3 for details.
    //
    // Step 2: Grab the records marked in step 1 and return the cursor to the caller. The
    // cursor will be translated directly to JSON without going through the model object form.
    //
    // Step 3: When the set of records has been successfully uploaded, an update is performed
    // do set all "in the laundry" records to either "clean" or back to dirty./ Note that
    // if a record has been updated in the meantime, it will have been marked dirty and won't
    // be selected for updating.
    //
    // The first two steps are handled in "getDirtyRows" and the last step is handled in
    // either "reWash" (in case of an error) or "setClean" (on successful upload).

    public static boolean isDirty(String tableName) {
        SQLiteStatement ss = mDb.compileStatement("select count(*) from " + tableName
                + " where " + KEY_DIRTY + " = 'Y'");
        long count = ss.simpleQueryForLong();
        Log.i(TAG, "Table " + tableName + " has " + count + " dirty rows.");
        return count > 0;
    }

    public static int countDirtyRowsForTable(String tableName) {
        SQLiteStatement ss = mDb.compileStatement("SELECT COUNT(*) FROM " + tableName
                + " where " + KEY_DIRTY + " = 'Y'");
        int count = (int) ss.simpleQueryForLong();
        Log.i(TAG, "Table " + tableName + " has " + count + " dirty rows.");
        return count;
    }

    public static Object getDirtyRowForTable(String tableName) {

        Object returnObject = null;

        // Grab dirty rows
        String query = "SELECT * FROM " + tableName + " WHERE " + KEY_DIRTY + " = 'Y' LIMIT 1";
        Cursor cursor = mDb.rawQuery(query, null);

        if (cursor.moveToFirst())
        {
            switch (tableName)
            {
                case RANDOMIZED_STUDENT_TABLE:
                    returnObject  = getRandomizedStudentFromCursor(cursor);
                    break;
                case RANDOMIZED_STUDENT_TRAY_TABLE:
                    returnObject = getRandomizedStudentTrayFromCursor(cursor);
                    break;
            }
        }


        return returnObject;
    }

    public static void reWash(String tableName) {
        String putBackInHamper = "update " + tableName + " set " + KEY_DIRTY + " = 1"
                + " where " + KEY_DIRTY + " = 2;";
        SQLiteStatement ss = mDb.compileStatement(putBackInHamper);
        ss.execute();
    }

    public static void setRowClean(String tableName, int id) {
        String foldAndPutAway = "UPDATE " + tableName + " SET " + KEY_DIRTY + " = 'N'"
                + " WHERE " + KEY_ROW_ID + " = " + id;
        SQLiteStatement ss = mDb.compileStatement(foldAndPutAway);
        ss.execute();
    }

    public static String[] getSyncableTables() {

        String[] tables = new String[] {
                RANDOMIZED_STUDENT_TABLE,
                RANDOMIZED_STUDENT_TRAY_TABLE
        };

        return tables;
    }

    // -------------------------------------------------------------------------------------
    // ------------------- School table access functions -----------------------------------
    // -------------------------------------------------------------------------------------
    // This method will see if the record id exist, if not, insert, if so, update.
    public static void insertSchool(School s) throws Exception{

        SQLiteStatement stmt = mDb.compileStatement("SELECT COUNT(*) FROM " + SCHOOL_TABLE + " WHERE " + KEY_ROW_ID + " = " + s.getId());
        long count = stmt.simpleQueryForLong();

        // To prevent Parcel from parsing null schoolLogo.
        if (s.getSchoolLogo() == null) s.setSchoolLogo(new byte[1]);

        // If the row does not exist, insert
        if(count == 0)
        {
            ContentValues args = toContentValues(s);

            long rowId = INVALID_ID;

            try {
                rowId = mDb.insert(SCHOOL_TABLE, null, args);
                if (rowId == INVALID_ID) {
                    Log.e(TAG, "Error inserting school: " + s.toString());
                    throw new Exception("Error inserting school: " + s.toString());
                }
            } catch (Exception e) {
                Log.e(TAG, "Error inserting school: " + s.toString(), e);
                throw e;
            }
        }
        // If it exist, update
        else
        {
            try {
                if(!updateSchool(s))
                {
                    Log.e(TAG, "Error updating school: " + s.toString());
                    throw new Exception("Error updating school: " + s.toString());
                }

            } catch (Exception e) {
                Log.e(TAG, "Error updating school: " + s.toString(), e);
                throw e;
            }
        }
    }

    public static void insertSchools(School[] ss)
    {
        mDb.beginTransaction();

        try {
            for(School s : ss)
            {
                insertSchool(s);
            }

            mDb.setTransactionSuccessful();
        }
        catch (Exception e) {
            Log.e(TAG, "Error inserting schools: ", e);
        }
        finally {
            mDb.endTransaction();
        }
    }

    public static School getSchool(int id)
    {
        School s = null;
        String constraint = KEY_ROW_ID + "=" + id;

        Cursor cursor = mDb.query(SCHOOL_TABLE,
                new String[] {
                        KEY_ROW_ID,
                        KEY_SCHOOL_NAME,
                        KEY_DISTRICT,
                        KEY_MASCOT,
                        KEY_COLORS,
                        KEY_SCHOOL_LOGO,
                        KEY_SCHOOL_TYPE_ID,
                        KEY_ACTIVE,

                        KEY_CREATE_TIMESTAMP,
                        KEY_CREATE_BY,
                        KEY_UPDATE_TIMESTAMP,
                        KEY_UPDATE_BY
                }, constraint, null, null, null, null);

        try
        {
            boolean found = cursor.moveToFirst();

            if(found)
            {
                s = getSchoolFromCursor(cursor);
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, "Caught DB exception", e);
        }
        finally
        {
            cursor.close();
        }

        return s;
    }

    public static ArrayList<School> getAllSchools()
    {
        ArrayList<School> returnArray = new ArrayList<School>();

        String orderBy = KEY_SCHOOL_NAME;

        Cursor cursor = mDb.query(SCHOOL_TABLE,
                new String[] {
                        KEY_ROW_ID,
                        KEY_SCHOOL_NAME,
                        KEY_DISTRICT,
                        KEY_MASCOT,
                        KEY_COLORS,
                        KEY_SCHOOL_LOGO,
                        KEY_SCHOOL_TYPE_ID,
                        KEY_ACTIVE,

                        KEY_CREATE_TIMESTAMP,
                        KEY_CREATE_BY,
                        KEY_UPDATE_TIMESTAMP,
                        KEY_UPDATE_BY
                }, null, null, null, null, KEY_SCHOOL_NAME);

        try
        {
            boolean found = cursor.moveToFirst();

            if(found)
            {
                do
                {
                    School s = getSchoolFromCursor(cursor);
                    returnArray.add(s);
                }while(cursor.moveToNext());
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, "Caught DB exception", e);
        }
        finally
        {
            cursor.close();
        }

        return returnArray;
    }

    public static boolean updateSchool(School s)
    {
        ContentValues args = toContentValues(s);
        String constraint = KEY_ROW_ID + "=" + s.getId();

        int rowsUpdated = mDb.update(SCHOOL_TABLE, args, constraint, null);

        return rowsUpdated > 0;
    }

    public static boolean deleteSchools()
    {
        try
        {
            mDb.delete(SCHOOL_TABLE, null, null);

            // Verify the delete.
            Cursor cc = mDb.query(SCHOOL_TABLE, null, null, null, null, null, null);
            if (cc != null) {
                if (cc.getCount() > 0) {
                    Log.e(TAG, "Attempted to delete * from: " + SCHOOL_TABLE + " but there were still records left.");
                    return false;
                }
            }

            return true;
        }
        catch (SQLException se)
        {
            Log.e(TAG, "Error deleting table: " + SCHOOL_TABLE, se);
            return false;
        }
    }

    private static ContentValues toContentValues(School s) {
        ContentValues args = new ContentValues();
        args.put(KEY_ROW_ID, s.getId());
        args.put(KEY_SCHOOL_NAME, s.getName());
        args.put(KEY_DISTRICT, s.getDistrict());
        args.put(KEY_MASCOT, s.getMascot());
        args.put(KEY_COLORS, s.getColors());
        args.put(KEY_SCHOOL_LOGO, s.getSchoolLogo());
        args.put(KEY_SCHOOL_TYPE_ID, s.getSchoolTypeId());
        args.put(KEY_ACTIVE, s.getActive());

        args.put(KEY_UPDATE_BY, s.getModifiedBy());

        return args;
    }

    private static School getSchoolFromCursor(Cursor cursor) {

        try {
            School s = new School(
                    cursor.getInt(cursor.getColumnIndexOrThrow(KEY_ROW_ID)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_SCHOOL_NAME)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_DISTRICT)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_MASCOT)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_COLORS)),
                    cursor.getBlob(cursor.getColumnIndexOrThrow(KEY_SCHOOL_LOGO)),
                    cursor.getInt(cursor.getColumnIndexOrThrow(KEY_SCHOOL_TYPE_ID)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_ACTIVE)),

                    dateFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_CREATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_CREATE_BY)),
                    dateFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_UPDATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_UPDATE_BY))
            );

            return s;
        } catch (ParseException e) {
            Log.e(TAG, "Error parsing String into Date", e);
            return null;
        }
    }

    // -------------------------------------------------------------------------------------
    // --------------- Intervention Day table access functions -----------------------------
    // -------------------------------------------------------------------------------------
    // This method will see if the record id exist, if not, insert, if so, update.
    public static void insertInterventionDay(InterventionDay itd) throws Exception{

        SQLiteStatement stmt = mDb.compileStatement("SELECT COUNT(*) FROM " + INTERVENTION_DAY_TABLE + " WHERE " + KEY_ROW_ID + " = " + itd.getId());
        long count = stmt.simpleQueryForLong();

        // If the row does not exist, insert
        if(count == 0)
        {
            ContentValues args = toContentValues(itd);

            long rowId = INVALID_ID;

            try {
                rowId = mDb.insert(INTERVENTION_DAY_TABLE, null, args);
                if (rowId == INVALID_ID) {
                    Log.e(TAG, "Error inserting intervention day: " + itd.toString());
                    throw new Exception("Error inserting intervention day: " + itd.toString());
                }
            } catch (Exception e) {
                Log.e(TAG, "Error inserting intervention day: " + itd.toString(), e);
                throw e;
            }
        }
        // If it exist, update
        else
        {
            try {
                if(!updateInterventionDay(itd))
                {
                    Log.e(TAG, "Error updating intervention day: " + itd.toString());
                    throw new Exception("Error updating intervention day: " + itd.toString());
                }

            } catch (Exception e) {
                Log.e(TAG, "Error updating intervention day: " + itd.toString(), e);
                throw e;
            }
        }
    }

    public static void insertInterventionDays(InterventionDay[] itds)
    {
        mDb.beginTransaction();

        try {
            for(InterventionDay itd : itds)
            {
                insertInterventionDay(itd);
            }

            mDb.setTransactionSuccessful();
        }
        catch (Exception e) {
            Log.e(TAG, "Error inserting intervention days: ", e);
        }
        finally {
            mDb.endTransaction();
        }
    }

    public static boolean updateInterventionDay(InterventionDay itd)
    {
        ContentValues args = toContentValues(itd);
        String constraint = KEY_ROW_ID + "=" + itd.getId();

        int rowsUpdated = mDb.update(INTERVENTION_DAY_TABLE, args, constraint, null);

        return rowsUpdated > 0;
    }

    public static InterventionDay getInterventionDay(int id)
    {
        InterventionDay itd = null;
        String constraint = KEY_ROW_ID + "=" + id;

        Cursor cursor = mDb.query(INTERVENTION_DAY_TABLE,
                new String[] {
                        KEY_ROW_ID,
                        KEY_SCHOOL_ID,
                        KEY_INTERVENTION_DATE,
                        KEY_INTERVENTION_FINISHED,
                        KEY_ACTIVE,

                        KEY_CREATE_TIMESTAMP,
                        KEY_CREATE_BY,
                        KEY_UPDATE_TIMESTAMP,
                        KEY_UPDATE_BY
                }, constraint, null, null, null, null);

        try
        {
            boolean found = cursor.moveToFirst();

            if(found)
            {
                itd = getInterventionDayFromCursor(cursor);
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, "Caught DB exception", e);
        }
        finally
        {
            cursor.close();
        }

        return itd;
    }

    public static ArrayList<InterventionDay> getAllInterventionDaysForSchool(int schoolId)
    {
        String constraint = KEY_SCHOOL_ID + "=" + schoolId + " AND " + KEY_INTERVENTION_FINISHED + "='N'";
        String orderBy = KEY_INTERVENTION_DATE;

        return getAllInterventionDaysWithConstraintAndOrder(constraint, orderBy);
    }

    public static ArrayList<InterventionDay> getAllUnfinishedInterventionDays()
    {
        String constraint = KEY_INTERVENTION_FINISHED + "='N'";

        return getAllInterventionDaysWithConstraint(constraint);
    }

    public static ArrayList<InterventionDay> getAllInterventionDaysWithConstraint(String constraint)
    {
        return getAllInterventionDaysWithConstraintAndOrder(constraint, null);
    }

    public static ArrayList<InterventionDay> getAllInterventionDaysWithConstraintAndOrder(String constraint, String orderBy)
    {
        ArrayList<InterventionDay> returnArray = new ArrayList();

        Cursor cursor = mDb.query(INTERVENTION_DAY_TABLE,
                new String[] {
                        KEY_ROW_ID,
                        KEY_SCHOOL_ID,
                        KEY_INTERVENTION_DATE,
                        KEY_INTERVENTION_FINISHED,
                        KEY_ACTIVE,

                        KEY_CREATE_TIMESTAMP,
                        KEY_CREATE_BY,
                        KEY_UPDATE_TIMESTAMP,
                        KEY_UPDATE_BY
                }, constraint, null, null, null, orderBy);

        try
        {
            boolean found = cursor.moveToFirst();

            if(found)
            {
                do
                {
                    InterventionDay rs = getInterventionDayFromCursor(cursor);
                    returnArray.add(rs);
                }while(cursor.moveToNext());
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, "Caught DB exception", e);
        }
        finally
        {
            cursor.close();
        }

        return returnArray;
    }

    public static boolean deleteInterventionDays()
    {
        try
        {
            mDb.delete(INTERVENTION_DAY_TABLE, null, null);

            // Verify the delete.
            Cursor cc = mDb.query(INTERVENTION_DAY_TABLE, null, null, null, null, null, null);
            if (cc != null) {
                if (cc.getCount() > 0) {
                    Log.e(TAG, "Attempted to delete * from: " + INTERVENTION_DAY_TABLE + " but there were still records left.");
                    return false;
                }
            }

            return true;
        }
        catch (SQLException se)
        {
            Log.e(TAG, "Error deleting table: " + INTERVENTION_DAY_TABLE, se);
            return false;
        }
    }

    private static ContentValues toContentValues(InterventionDay itd) {

        ContentValues args = new ContentValues();
        args.put(KEY_ROW_ID, itd.getId());
        args.put(KEY_SCHOOL_ID, itd.getSchoolId());
        args.put(KEY_INTERVENTION_DATE, dateOnlyFormatter.format(itd.getInterventionDate()));
        args.put(KEY_INTERVENTION_FINISHED, itd.getInterventionFinished());
        args.put(KEY_ACTIVE, itd.getActive());

        args.put(KEY_UPDATE_BY, itd.getModifiedBy());

        return args;
    }

    private static InterventionDay getInterventionDayFromCursor(Cursor cursor) {

        try {
            InterventionDay itd = new InterventionDay(
                    cursor.getInt(cursor.getColumnIndexOrThrow(KEY_ROW_ID)),
                    cursor.getInt(cursor.getColumnIndexOrThrow(KEY_SCHOOL_ID)),
                    dateOnlyFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_INTERVENTION_DATE))),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_INTERVENTION_FINISHED)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_ACTIVE)),

                    dateFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_CREATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_CREATE_BY)),
                    dateFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_UPDATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_UPDATE_BY))
            );

            return itd;
        } catch (ParseException e) {
            Log.e(TAG, "Error parsing String into Date", e);
            return null;
        }
    }

    // -------------------------------------------------------------------------------------
    // --------------- Randomized Student table access functions ---------------------------
    // -------------------------------------------------------------------------------------
    public static void insertRandomizedStudent(RandomizedStudent rs) throws Exception{

        RandomizedStudent randomizedStudentFromDb = null;
        boolean found;

        String constraint = KEY_ROW_ID + "=" + rs.getId();

        Cursor cursor = mDb.query(RANDOMIZED_STUDENT_TABLE,
                new String[] {
                        KEY_ROW_ID,
                        KEY_SCHOOL_ID,
                        KEY_INTERVENTION_DAY_ID,
                        KEY_STUDENT_ID,
                        KEY_GENDER,
                        KEY_GRADE,
                        KEY_ASSENT,
                        KEY_DIRTY,

                        KEY_CREATE_TIMESTAMP,
                        KEY_CREATE_BY,
                        KEY_UPDATE_TIMESTAMP,
                        KEY_UPDATE_BY
                }, constraint, null, null, null, null);

        found = cursor.moveToFirst();

        // If the row does not exist, insert
        if(!found) {

            // Setting dirty to No, since new randomized students should be directly from the server.
            rs.setDirty("N");
            ContentValues args = toContentValues(rs);

            long rowId;

            try {
                rowId = mDb.insert(RANDOMIZED_STUDENT_TABLE, null, args);
                if (rowId == INVALID_ID) {
                    Log.e(TAG, "Error inserting randomized student: " + rs.toString());
                    throw new Exception("Error inserting randomized student: " + rs.toString());
                }
            } catch (Exception e) {
                Log.e(TAG, "Error inserting randomized student: " + rs.toString(), e);
                throw e;
            }
        }

        // If it exist, update
        else {
            try {

                randomizedStudentFromDb = getRandomizedStudentFromCursor(cursor);

                // Check to see if server's data is more up-to-date
                if (rs.getDtModified() != null && rs.getDtModified().after(randomizedStudentFromDb.getDtModified())) {
                    // Check to see if the current db version is dirty
                    if (randomizedStudentFromDb.getDirty().equalsIgnoreCase("N")) {
                        if(!updateRandomizedStudent(rs, "N")) {
                            Log.e(TAG, "Error updating randomized student: " + rs.toString());
                            throw new Exception("Error updating randomized student: " + rs.toString());
                        }
                    }
                    // This shouldn't have happened.
                    else {
                        Log.e(TAG, "Randomized student is still dirty after pushing to server: " + randomizedStudentFromDb.toString());
                        throw new Exception("Randomized student is still dirty after pushing to server: " + randomizedStudentFromDb.toString());
                    }
                }
            }
            catch (Exception e) {
                Log.e(TAG, "Error updating randomized student: " + rs.toString(), e);
                throw e;
            }
            finally {
                cursor.close();
            }
        }
    }

    public static void insertRandomizedStudents(RandomizedStudent[] rss)
    {
        mDb.beginTransaction();

        try {
            for(RandomizedStudent rs : rss)
            {
                insertRandomizedStudent(rs);
            }

            mDb.setTransactionSuccessful();
        }
        catch (Exception e) {
            Log.e(TAG, "Error inserting randomized students: ", e);
        }
        finally {
            mDb.endTransaction();
        }
    }

    public static ArrayList<RandomizedStudent> getRandomizedStudentsForSchoolAndInterventionDays(int schoolId, int interventionDayId)
    {
        ArrayList<RandomizedStudent> returnArray = new ArrayList<RandomizedStudent>();

        String constraint = KEY_SCHOOL_ID + "=" + schoolId + " and " + KEY_INTERVENTION_DAY_ID + "=" + interventionDayId;

        Cursor cursor = mDb.query(RANDOMIZED_STUDENT_TABLE,
                new String[] {
                        KEY_ROW_ID,
                        KEY_SCHOOL_ID,
                        KEY_INTERVENTION_DAY_ID,
                        KEY_STUDENT_ID,
                        KEY_GENDER,
                        KEY_GRADE,
                        KEY_ASSENT,
                        KEY_DIRTY,

                        KEY_CREATE_TIMESTAMP,
                        KEY_CREATE_BY,
                        KEY_UPDATE_TIMESTAMP,
                        KEY_UPDATE_BY
                }, constraint, null, null, null, null);

        try
        {
            boolean found = cursor.moveToFirst();

            if(found)
            {
                do
                {
                    RandomizedStudent rs = getRandomizedStudentFromCursor(cursor);
                    returnArray.add(rs);
                }while(cursor.moveToNext());
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, "Caught DB exception", e);
        }
        finally
        {
            cursor.close();
        }

        return returnArray;
    }

    public static ArrayList<RandomizedStudent> getAllRandomizedStudents()
    {
        ArrayList<RandomizedStudent> returnArray = new ArrayList<RandomizedStudent>();

        Cursor cursor = mDb.query(RANDOMIZED_STUDENT_TABLE,
                new String[] {
                        KEY_ROW_ID,
                        KEY_SCHOOL_ID,
                        KEY_INTERVENTION_DAY_ID,
                        KEY_STUDENT_ID,
                        KEY_GENDER,
                        KEY_GRADE,
                        KEY_ASSENT,
                        KEY_DIRTY,

                        KEY_CREATE_TIMESTAMP,
                        KEY_CREATE_BY,
                        KEY_UPDATE_TIMESTAMP,
                        KEY_UPDATE_BY
                }, null, null, null, null, null);

        try
        {
            boolean found = cursor.moveToFirst();

            if(found)
            {
                do
                {
                    RandomizedStudent rs = getRandomizedStudentFromCursor(cursor);
                    returnArray.add(rs);
                }while(cursor.moveToNext());
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, "Caught DB exception", e);
        }
        finally
        {
            cursor.close();
        }

        return returnArray;
    }

    public static int getAssentedCount(int schoolId, int interventionDayId)
    {
        SQLiteStatement ss = mDb.compileStatement("SELECT COUNT(*) FROM " + RANDOMIZED_STUDENT_TABLE
                + " WHERE " + KEY_SCHOOL_ID + "=" + schoolId + " AND " + KEY_INTERVENTION_DAY_ID + "=" + interventionDayId
                + " AND " + KEY_ASSENT + "= 'Y'");
        Long count = ss.simpleQueryForLong();
        return count.intValue();
    }

    public static int getTotalRandomizedStudentCount()
    {
        SQLiteStatement ss = mDb.compileStatement("SELECT COUNT(*) FROM " + RANDOMIZED_STUDENT_TABLE);
        Long count = ss.simpleQueryForLong();
        return count.intValue();
    }

    public static int getTotalFutureRandomizedStudentCount()
    {
        Date today = Calendar.getInstance().getTime();
        SimpleDateFormat spf= new SimpleDateFormat("yyyy-MM-dd");
        String todayStr = spf.format(today);

        String constraint =  KEY_INTERVENTION_DATE + ">='" + todayStr + "'";
        String orderBy = KEY_ROW_ID + " desc";
        ArrayList<InterventionDay> itds = getAllInterventionDaysWithConstraintAndOrder(constraint, orderBy);
        int interventionDayId = Integer.MAX_VALUE;
        if (itds.size() > 0) {
            interventionDayId = itds.get(0).getId();
        }

        SQLiteStatement ss = mDb.compileStatement("SELECT COUNT(*) FROM " + RANDOMIZED_STUDENT_TABLE
                + " WHERE " + KEY_INTERVENTION_DAY_ID + ">=" + interventionDayId);
        Long count = ss.simpleQueryForLong();
        return count.intValue();
    }

    public static boolean updateRandomizedStudent(RandomizedStudent rs, String dirty)
    {
        rs.setDirty(dirty);

        ContentValues args = toContentValues(rs);
        String constraint = KEY_ROW_ID + "=" + rs.getId();

        int rowsUpdated = mDb.update(RANDOMIZED_STUDENT_TABLE, args, constraint, null);

        return rowsUpdated > 0;
    }

    public static boolean deleteRandomizedStudents()
    {
        try
        {
            mDb.delete(RANDOMIZED_STUDENT_TABLE, null, null);

            // Verify the delete.
            Cursor cc = mDb.query(RANDOMIZED_STUDENT_TABLE, null, null, null, null, null, null);
            if (cc != null) {
                if (cc.getCount() > 0) {
                    Log.e(TAG, "Attempted to delete * from: " + RANDOMIZED_STUDENT_TABLE + " but there were still records left.");
                    return false;
                }
            }

            return true;
        }
        catch (SQLException se)
        {
            Log.e(TAG, "Error deleting table: " + RANDOMIZED_STUDENT_TABLE, se);
            return false;
        }
    }

    private static ContentValues toContentValues(RandomizedStudent rs) {
        ContentValues args = new ContentValues();
        args.put(KEY_ROW_ID, rs.getId());
        args.put(KEY_SCHOOL_ID, rs.getSchoolId());
        args.put(KEY_INTERVENTION_DAY_ID, rs.getInterventionDayId());
        args.put(KEY_STUDENT_ID, rs.getStudentId());
        args.put(KEY_GENDER, rs.getGender());
        args.put(KEY_GRADE, rs.getGrade());
        args.put(KEY_ASSENT, rs.getAssent());
        args.put(KEY_DIRTY, rs.getDirty());

        args.put(KEY_UPDATE_BY, rs.getModifiedBy());

        return args;
    }

    private static RandomizedStudent getRandomizedStudentFromCursor(Cursor cursor) {

        try
        {
            RandomizedStudent rs = new RandomizedStudent(
                    cursor.getInt(cursor.getColumnIndexOrThrow(KEY_ROW_ID)),
                    cursor.getInt(cursor.getColumnIndexOrThrow(KEY_SCHOOL_ID)),
                    cursor.getInt(cursor.getColumnIndexOrThrow(KEY_INTERVENTION_DAY_ID)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_STUDENT_ID)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_GENDER)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_GRADE)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_ASSENT)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_DIRTY)),

                    dateFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_CREATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_CREATE_BY)),
                    dateFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_UPDATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_UPDATE_BY))
            );

            return rs;
        } catch (ParseException e) {
            Log.e(TAG, "Error parsing String into Date", e);
            return null;
        }
    }

    public static boolean temporarySetDirtyToNForNullAssent() {
        SQLiteStatement stmt = mDb.compileStatement("SELECT COUNT(*) FROM " + RANDOMIZED_STUDENT_TABLE + " WHERE " + KEY_DIRTY + " = 'Y' AND " + KEY_ASSENT + " IS NULL");
        long beginCount = stmt.simpleQueryForLong();
        Log.i(TAG, Long.toString(beginCount));

        if (beginCount > 0) {
            mDb.execSQL("UPDATE " + RANDOMIZED_STUDENT_TABLE + " SET " + KEY_DIRTY + " = 'N' WHERE " + KEY_ASSENT + " IS NULL");

            stmt = mDb.compileStatement("SELECT COUNT(*) FROM " + RANDOMIZED_STUDENT_TABLE + " WHERE " + KEY_DIRTY + " = 'Y' AND " + KEY_ASSENT + " IS NULL");
            long endCount = stmt.simpleQueryForLong();
            Log.i(TAG, Long.toString(endCount));

            return endCount == 0;
        }

        return true;
    }

    // -------------------------------------------------------------------------------------
    // ------------------- ResearchTeamMember table access functions -----------------------
    // -------------------------------------------------------------------------------------
    // This method will see if the record id exist, if not, insert, if so, update.
    public static void insertResearchTeamMember(ResearchTeamMember r) throws Exception{

        SQLiteStatement stmt = mDb.compileStatement("SELECT COUNT(*) FROM " + RESEARCH_TEAM_MEMBER_TABLE + " WHERE " + KEY_ROW_ID + " = " + r.getId());
        long count = stmt.simpleQueryForLong();

        // If the row does not exist, insert
        if(count == 0)
        {
            ContentValues args = toContentValues(r);

            long rowId = INVALID_ID;

            try {
                rowId = mDb.insert(RESEARCH_TEAM_MEMBER_TABLE, null, args);
                if (rowId == INVALID_ID) {
                    Log.e(TAG, "Error inserting research team member: " + r.toString());
                    throw new Exception("Error inserting research team member: " + r.toString());
                }
            } catch (Exception e) {
                Log.e(TAG, "Error inserting research team member: " + r.toString(), e);
                throw e;
            }
        }
        // If it exist, update
        else
        {
            try {
                if(!updateResearchTeamMember(r))
                {
                    Log.e(TAG, "Error updating research team member: " + r.toString());
                    throw new Exception("Error updating research team member: " + r.toString());
                }

            } catch (Exception e) {
                Log.e(TAG, "Error updating research team member: " + r.toString(), e);
                throw e;
            }
        }
    }

    public static void insertResearcherTeamMembers(ResearchTeamMember[] rs)
    {
        mDb.beginTransaction();

        try {
            for(ResearchTeamMember r : rs)
            {
                insertResearchTeamMember(r);
            }

            mDb.setTransactionSuccessful();
        }
        catch (Exception e) {
            Log.e(TAG, "Error inserting research team members: ", e);
        }
        finally {
            mDb.endTransaction();
        }
    }

    public static ArrayList<ResearchTeamMember> getAllResearchTeamMembers()
    {
        ArrayList<ResearchTeamMember> returnArray = new ArrayList<ResearchTeamMember>();

        String constraint = KEY_ACTIVE + "='Y'";
        String orderBy = KEY_RESEARCHER_LAST_NAME + ", " + KEY_RESEARCHER_FIRST_NAME;

        Cursor cursor = mDb.query(RESEARCH_TEAM_MEMBER_TABLE,
                new String[] {
                        KEY_ROW_ID,
                        KEY_RESEARCHER_EMAIL,
                        KEY_RESEARCHER_FIRST_NAME,
                        KEY_RESEARCHER_LAST_NAME,
                        KEY_ACTIVE,

                        KEY_CREATE_TIMESTAMP,
                        KEY_CREATE_BY,
                        KEY_UPDATE_TIMESTAMP,
                        KEY_UPDATE_BY
                }, constraint, null, null, null, orderBy);

        try
        {
            boolean found = cursor.moveToFirst();

            if(found)
            {
                do
                {
                    ResearchTeamMember r = getResearcherFromCursor(cursor);
                    returnArray.add(r);
                }while(cursor.moveToNext());
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, "Caught DB exception", e);
        }
        finally
        {
            cursor.close();
        }

        return returnArray;
    }

    public static boolean updateResearchTeamMember(ResearchTeamMember rtm)
    {
        ContentValues args = toContentValues(rtm);
        String constraint = KEY_ROW_ID + "=" + rtm.getId();

        int rowsUpdated = mDb.update(RESEARCH_TEAM_MEMBER_TABLE, args, constraint, null);

        return rowsUpdated > 0;
    }

    public static boolean deleteResearchers()
    {
        try
        {
            mDb.delete(RESEARCH_TEAM_MEMBER_TABLE, null, null);

            // Verify the delete.
            Cursor cc = mDb.query(RESEARCH_TEAM_MEMBER_TABLE, null, null, null, null, null, null);
            if (cc != null) {
                if (cc.getCount() > 0) {
                    Log.e(TAG, "Attempted to delete * from: " + RESEARCH_TEAM_MEMBER_TABLE + " but there were still records left.");
                    return false;
                }
            }

            return true;
        }
        catch (SQLException se)
        {
            Log.e(TAG, "Error deleting table: " + RESEARCH_TEAM_MEMBER_TABLE, se);
            return false;
        }
    }

    private static ContentValues toContentValues(ResearchTeamMember r) {
        ContentValues args = new ContentValues();
        args.put(KEY_ROW_ID, r.getId());
        args.put(KEY_RESEARCHER_EMAIL, r.getEmail());
        args.put(KEY_RESEARCHER_FIRST_NAME, r.getFirstName());
        args.put(KEY_RESEARCHER_LAST_NAME, r.getLastName());
        args.put(KEY_ACTIVE, r.getActive());

        args.put(KEY_UPDATE_BY, r.getModifiedBy());

        return args;
    }

    private static ResearchTeamMember getResearcherFromCursor(Cursor cursor) {

        try
        {
            ResearchTeamMember r = new ResearchTeamMember(
                    cursor.getInt(cursor.getColumnIndexOrThrow(KEY_ROW_ID)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_RESEARCHER_EMAIL)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_RESEARCHER_FIRST_NAME)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_RESEARCHER_LAST_NAME)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_ACTIVE)),

                    dateFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_CREATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_CREATE_BY)),
                    dateFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_UPDATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_UPDATE_BY))
            );

            return r;
        } catch (ParseException e) {
            Log.e(TAG, "Error parsing String into Date", e);
            return null;
        }
    }

    // -------------------------------------------------------------------------------------
    // ------------------- RandomizedStudentTray table access functions --------------------
    // -------------------------------------------------------------------------------------
    public static void insertRandomizedStudentTray(RandomizedStudentTray rst) throws Exception{

        ContentValues args = toContentValues(rst);

        long rowId;

        try {
            rowId = mDb.insert(RANDOMIZED_STUDENT_TRAY_TABLE, null, args);
            if (rowId == INVALID_ID) {
                Log.e(TAG, "Error inserting randomized student tray: " + rst.toString());
                throw new Exception("Error inserting randomized student tray: " + rst.toString());
            }
        } catch (Exception e) {
            Log.e(TAG, "Error inserting randomized student tray: " + rst.toString(), e);
            throw e;
        }
    }

    public static void insertRandomizedStudentTrays(ArrayList<RandomizedStudentTray> rsts)
    {
        mDb.beginTransaction();

        try {
            for(RandomizedStudentTray rst : rsts)
            {
                insertRandomizedStudentTray(rst);
            }

            mDb.setTransactionSuccessful();
        }
        catch (Exception e) {
            Log.e(TAG, "Error inserting randomized student trays: ", e);
        }
        finally {
            mDb.endTransaction();
        }
    }

    public static ArrayList<RandomizedStudentTray> getRandomizedStudentTraysForStudent(int randomizedStudentRowId)
    {
        ArrayList<RandomizedStudentTray> returnArray = new ArrayList<>();

        String constraint = KEY_RANDOMIZED_STUDENT_ID + "=" + randomizedStudentRowId;

        String orderBy =  KEY_CREATE_TIMESTAMP + " DESC";

        Cursor cursor = mDb.query(RANDOMIZED_STUDENT_TRAY_TABLE,
                new String[] {
                        KEY_ROW_ID,
                        KEY_TRAY_ID,
                        KEY_RANDOMIZED_STUDENT_ID,
                        KEY_DIRTY,

                        KEY_CREATE_TIMESTAMP,
                        KEY_CREATE_BY,
                        KEY_UPDATE_TIMESTAMP,
                        KEY_UPDATE_BY
                }, constraint, null, null, null, orderBy);

        try
        {
            boolean found = cursor.moveToFirst();

            if(found)
            {
                do
                {
                    RandomizedStudentTray rst = getRandomizedStudentTrayFromCursor(cursor);
                    returnArray.add(rst);
                }while(cursor.moveToNext());
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, "Caught DB exception", e);
        }
        finally
        {
            cursor.close();
        }

        return returnArray;
    }

    public static boolean deleteRandomizedStudentTray()
    {
        try
        {
            mDb.delete(RANDOMIZED_STUDENT_TRAY_TABLE, null, null);

            // Verify the delete.
            Cursor cc = mDb.query(RANDOMIZED_STUDENT_TRAY_TABLE, null, null, null, null, null, null);
            if (cc != null) {
                if (cc.getCount() > 0) {
                    Log.e(TAG, "Attempted to delete * from: " + RANDOMIZED_STUDENT_TRAY_TABLE + " but there were still records left.");
                    return false;
                }
            }

            return true;
        }
        catch (SQLException se)
        {
            Log.e(TAG, "Error deleting table: " + RANDOMIZED_STUDENT_TRAY_TABLE, se);
            return false;
        }
    }

    private static ContentValues toContentValues(RandomizedStudentTray rst) {
        ContentValues args = new ContentValues();
        args.put(KEY_TRAY_ID, rst.getTrayId());
        args.put(KEY_RANDOMIZED_STUDENT_ID, rst.getRandomizedStudentId());
        args.put(KEY_DIRTY, rst.getDirty());

        args.put(KEY_CREATE_BY, rst.getCreatedBy());
        args.put(KEY_UPDATE_BY, rst.getModifiedBy());

        return args;
    }

    private static RandomizedStudentTray getRandomizedStudentTrayFromCursor(Cursor cursor) {

        try
        {
            RandomizedStudentTray rst = new RandomizedStudentTray(
                    cursor.getInt(cursor.getColumnIndexOrThrow(KEY_ROW_ID)),
                    cursor.getLong(cursor.getColumnIndexOrThrow(KEY_TRAY_ID)),
                    cursor.getInt(cursor.getColumnIndexOrThrow(KEY_RANDOMIZED_STUDENT_ID)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_DIRTY)),

                    dateFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_CREATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_CREATE_BY)),
                    dateFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_UPDATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_UPDATE_BY))
            );

            return rst;
        } catch (ParseException e) {
            Log.e(TAG, "Error parsing String into Date", e);
            return null;
        }
    }

    // -------------------------------------------------------------------------------------
    // ------------------- Device table access functions -----------------------------------
    // -------------------------------------------------------------------------------------
    // This method will see if the record id exist, if not, insert, if so, update.
    public static void insertDevice(Device d) {

        SQLiteStatement stmt = mDb.compileStatement("SELECT COUNT(*) FROM " + DEVICE_TABLE + " WHERE " + KEY_DEVICE_ID + " = '" + d.getDeviceId() + "'");
        long count = stmt.simpleQueryForLong();

        // If the row does not exist, insert
        if(count == 0)
        {
            ContentValues args = toContentValues(d);

            long rowId = INVALID_ID;

            try {
                rowId = mDb.insert(DEVICE_TABLE, null, args);
                if (rowId == INVALID_ID) {
                    Log.e(TAG, "Error inserting device: " + d.toString());
                    mDb.endTransaction();
                }
            } catch (Exception e) {
                Log.e(TAG, "Error inserting device: " + d.toString(), e);
                mDb.endTransaction();
            }
        }
        // If it exist, update
        else
        {
            try {
                if(!updateDevice(d))
                {
                    Log.e(TAG, "Error updating device: " + d.toString());
                    mDb.endTransaction();
                }

            } catch (Exception e) {
                Log.e(TAG, "Error updating device: " + d.toString(), e);
                mDb.endTransaction();
            }
        }
    }

    public static Device getDevice()
    {
        Device d = null;
        Cursor cursor = mDb.query(DEVICE_TABLE,
                new String[] {
                        KEY_ROW_ID,
                        KEY_DEVICE_ID,
                        KEY_DEVICE_NAME,
                        KEY_DIRTY,

                        KEY_CREATE_TIMESTAMP,
                        KEY_CREATE_BY,
                        KEY_UPDATE_TIMESTAMP,
                        KEY_UPDATE_BY
                }, null, null, null, null, null);

        try
        {
            boolean found = cursor.moveToFirst();

            if (found)
            {
                d = getDeviceFromCursor(cursor);
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, "Caught DB exception", e);
        }
        finally
        {
            cursor.close();
        }

        return d;
    }


    public static ArrayList<Device> getAllDevices()
    {
        ArrayList<Device> returnArray = new ArrayList<>();

        Cursor cursor = mDb.query(DEVICE_TABLE,
                new String[] {
                        KEY_ROW_ID,
                        KEY_DEVICE_ID,
                        KEY_DEVICE_NAME,
                        KEY_DIRTY,

                        KEY_CREATE_TIMESTAMP,
                        KEY_CREATE_BY,
                        KEY_UPDATE_TIMESTAMP,
                        KEY_UPDATE_BY
                }, null, null, null, null, null);

        try
        {
            boolean found = cursor.moveToFirst();

            if(found)
            {
                do
                {
                    Device d = getDeviceFromCursor(cursor);
                    returnArray.add(d);
                }while(cursor.moveToNext());
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, "Caught DB exception", e);
        }
        finally
        {
            cursor.close();
        }

        return returnArray;
    }

    public static boolean updateDevice(Device d)
    {
        ContentValues args = toContentValues(d);
        String constraint = KEY_ROW_ID + "=" + d.getId();

        int rowsUpdated = mDb.update(DEVICE_TABLE, args, constraint, null);

        return rowsUpdated > 0;
    }

    private static ContentValues toContentValues(Device d) {
        ContentValues args = new ContentValues();
        args.put(KEY_ROW_ID, d.getId());
        args.put(KEY_DEVICE_ID, d.getDeviceId());
        args.put(KEY_DEVICE_NAME, d.getDeviceName());
        args.put(KEY_DIRTY, d.getDirty());

        if (d.getCreatedBy() != null) {
            args.put(KEY_CREATE_BY, d.getCreatedBy());
        }

        if (d.getModifiedBy() != null) {
            args.put(KEY_UPDATE_BY, d.getModifiedBy());
        }

        return args;
    }

    private static Device getDeviceFromCursor(Cursor cursor) {

        try
        {
            Device d = new Device(
                    cursor.getInt(cursor.getColumnIndexOrThrow(KEY_ROW_ID)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_DEVICE_ID)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_DEVICE_NAME)),
                    cursor.getString(cursor.getColumnIndex(KEY_DIRTY)),

                    dateFormatter.parse(cursor.getString(cursor.getColumnIndex(KEY_CREATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndex(KEY_CREATE_BY)),
                    dateFormatter.parse(cursor.getString(cursor.getColumnIndex(KEY_UPDATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndex(KEY_UPDATE_BY))
            );

            return d;
        } catch (ParseException e) {
            Log.e(TAG, "Error parsing String into Date", e);
            return null;
        }
    }

    // -------------------------------------------------------------------------------------
    // ------------------- Audit table access functions -----------------------------------
    // -------------------------------------------------------------------------------------
    public static long insertAudit(Audit a) {

        ContentValues args = toContentValues(a);

        long rowId = INVALID_ID;

        try {
            rowId = mDb.insert(AUDIT_TABLE, null, args);
            if (rowId == INVALID_ID) {
                Log.e(TAG, "Error inserting audit: " + a.toString());
            }
        } catch (Exception e) {
            Log.e(TAG, "Error inserting audit: " + a.toString(), e);
        }

        return rowId;
    }

    public static ArrayList<Audit> getAllAudits()
    {
        ArrayList<Audit> returnArray = new ArrayList<>();

        Cursor cursor = mDb.query(AUDIT_TABLE,
                new String[] {
                        KEY_ROW_ID,
                        KEY_TABLE_NAME,
                        KEY_ACTION,
                        KEY_VALUE_BEFORE,
                        KEY_VALUE_AFTER,
                        KEY_DIRTY,

                        KEY_CREATE_TIMESTAMP,
                        KEY_CREATE_BY,
                        KEY_UPDATE_TIMESTAMP,
                        KEY_UPDATE_BY
                }, null, null, null, null, null);

        try
        {
            boolean found = cursor.moveToFirst();

            if(found)
            {
                do
                {
                    Audit a = getAuditFromCursor(cursor);
                    returnArray.add(a);
                }while(cursor.moveToNext());
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, "Caught DB exception", e);
        }
        finally
        {
            cursor.close();
        }

        return returnArray;
    }

    private static ContentValues toContentValues(Audit a) {
        ContentValues args = new ContentValues();
        args.put(KEY_TABLE_NAME, a.getTableName());
        args.put(KEY_ACTION, a.getAction());
        args.put(KEY_VALUE_BEFORE, a.getValueBefore());
        args.put(KEY_VALUE_AFTER, a.getValueAfter());
        args.put(KEY_DIRTY, a.getDirty());

        args.put(KEY_CREATE_BY, a.getCreatedBy());
        args.put(KEY_UPDATE_BY, a.getModifiedBy());

        return args;
    }

    private static Audit getAuditFromCursor(Cursor cursor) {

        try {
            Audit a = new Audit(
                    cursor.getInt(cursor.getColumnIndexOrThrow(KEY_ROW_ID)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_TABLE_NAME)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_ACTION)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_VALUE_BEFORE)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_VALUE_AFTER)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_DIRTY)),

                    dateFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_CREATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_CREATE_BY)),
                    dateFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_UPDATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_UPDATE_BY))
                    );

            return a;
        } catch (ParseException e) {
            Log.e(TAG, "Error parsing String into Date", e);
            return null;
        }
    }


    // -------------------------------------------------------------------------------------
    // ------------------- Token table access functions -----------------------------------
    // -------------------------------------------------------------------------------------
    public static long insertToken(Token t) {

        ContentValues args = toContentValues(t);

        long rowId = INVALID_ID;

        try {
            rowId = mDb.insert(TOKEN_TABLE, null, args);
            if (rowId == INVALID_ID) {
                Log.e(TAG, "Error inserting token: " + t.toString());
            }
        } catch (Exception e) {
            Log.e(TAG, "Error inserting token: " + t.toString(), e);
        }

        return rowId;
    }



    public static ArrayList<Token> getAllTokens()
    {
        ArrayList<Token> returnArray = new ArrayList<>();

        Cursor cursor = mDb.query(TOKEN_TABLE,
                new String[] {
                        KEY_ROW_ID,
                        KEY_RESEARCHER_EMAIL,
                        KEY_TOKEN,
                        KEY_TOKEN_EXPIRATION_TIME,

                        KEY_CREATE_TIMESTAMP,
                        KEY_CREATE_BY,
                        KEY_UPDATE_TIMESTAMP,
                        KEY_UPDATE_BY
                }, null, null, null, null, null);

        try
        {
            boolean found = cursor.moveToFirst();

            if(found)
            {
                do
                {
                    Token t = getTokenFromCursor(cursor);
                    returnArray.add(t);
                }while(cursor.moveToNext());
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, "Caught DB exception", e);
        }
        finally
        {
            cursor.close();
        }

        return returnArray;
    }

    private static ContentValues toContentValues(Token t) {
        ContentValues args = new ContentValues();
        args.put(KEY_RESEARCHER_EMAIL, t.getEmail());
        args.put(KEY_TOKEN, t.getToken());
        args.put(KEY_TOKEN_EXPIRATION_TIME, dateFormatter.format(t.getTokenExpirationTime()));

        return args;
    }

    private static Token getTokenFromCursor(Cursor cursor) {

        try {
            Token t = new Token(
                    cursor.getInt(cursor.getColumnIndexOrThrow(KEY_ROW_ID)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_RESEARCHER_EMAIL)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_TOKEN)),
                    dateFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_TOKEN_EXPIRATION_TIME))),

                    dateFormatter.parse(cursor.getString(cursor.getColumnIndex(KEY_CREATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndex(KEY_CREATE_BY)),
                    dateFormatter.parse(cursor.getString(cursor.getColumnIndex(KEY_UPDATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndex(KEY_UPDATE_BY))
            );

            return t;
        } catch (ParseException e) {
            Log.e(TAG, "Error parsing String into Date", e);
            return null;
        }
    }


    // -------------------------------------------------------------------------------------
    // ------------------- Tracking table access functions -----------------------------------
    // -------------------------------------------------------------------------------------
    public static long insertTracking(Tracking t) {

        ContentValues args = toContentValues(t);

        long rowId = INVALID_ID;

        try {
            rowId = mDb.insert(TRACKING_TABLE, null, args);
            if (rowId == INVALID_ID) {
                Log.e(TAG, "Error inserting tracking: " + t.toString());
            }
        } catch (Exception e) {
            Log.e(TAG, "Error inserting tracking: " + t.toString(), e);
        }

        return rowId;
    }

    public static ArrayList<Tracking> getAllTrackings()
    {
        ArrayList<Tracking> returnArray = new ArrayList<>();

        Cursor cursor = mDb.query(TRACKING_TABLE,
                new String[] {
                        KEY_ROW_ID,
                        KEY_INPUT,
                        KEY_LOCATION,
                        KEY_SCHOOL_NAME,
                        KEY_INTERVENTION_DATE,

                        KEY_CREATE_TIMESTAMP,
                        KEY_CREATE_BY
                }, null, null, null, null, null);

        try
        {
            boolean found = cursor.moveToFirst();

            if(found)
            {
                do
                {
                    Tracking t = getTrackingFromCursor(cursor);
                    returnArray.add(t);
                }while(cursor.moveToNext());
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, "Caught DB exception", e);
        }
        finally
        {
            cursor.close();
        }

        return returnArray;
    }

    private static ContentValues toContentValues(Tracking t) {
        ContentValues args = new ContentValues();
        args.put(KEY_INPUT, t.getInput());
        args.put(KEY_LOCATION, t.getLocation());
        args.put(KEY_SCHOOL_NAME, t.getSchool());
        args.put(KEY_INTERVENTION_DATE, dateOnlyFormatter.format(t.getInterventionDay()));

        args.put(KEY_CREATE_BY, t.getCreatedBy());

        return args;
    }

    private static Tracking getTrackingFromCursor(Cursor cursor) {

        try {
            Tracking t = new Tracking(
                    cursor.getInt(cursor.getColumnIndexOrThrow(KEY_ROW_ID)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_INPUT)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_LOCATION)),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_SCHOOL_NAME)),
                    dateOnlyFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_INTERVENTION_DATE))),

                    dateFormatter.parse(cursor.getString(cursor.getColumnIndexOrThrow(KEY_CREATE_TIMESTAMP))),
                    cursor.getString(cursor.getColumnIndexOrThrow(KEY_CREATE_BY))
            );

            return t;
        } catch (ParseException e) {
            Log.e(TAG, "Error parsing String into Date", e);
            return null;
        }
    }
}
