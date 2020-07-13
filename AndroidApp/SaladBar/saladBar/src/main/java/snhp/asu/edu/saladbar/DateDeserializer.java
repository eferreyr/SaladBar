package snhp.asu.edu.saladbar;

import android.util.Log;

import com.google.gson.JsonDeserializationContext;
import com.google.gson.JsonDeserializer;
import com.google.gson.JsonElement;
import com.google.gson.JsonParseException;

import java.lang.reflect.Type;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.TimeZone;

/**
 * Created by John on 2/21/18.
 */

public class DateDeserializer implements JsonDeserializer<Date> {
    private static final String TAG = "asu-snhp";

    @Override
    public Date deserialize(JsonElement element, Type arg1, JsonDeserializationContext arg2) throws JsonParseException {
        String date = element.getAsString();

        SimpleDateFormat formatter;

        if (date.matches(".*\\.\\d+"))
        {
            formatter = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS");

        }

        else
        {
            formatter = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");
        }

        formatter.setTimeZone(TimeZone.getTimeZone("America/Phoenix"));



        try {
            return formatter.parse(date);
        } catch (ParseException e) {
            Log.e(TAG, "Failed to parse Date due to:", e);
            return null;
        }
    }
}

