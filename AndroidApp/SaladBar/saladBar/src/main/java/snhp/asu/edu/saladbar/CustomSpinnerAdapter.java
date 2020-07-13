package snhp.asu.edu.saladbar;

import android.content.Context;
import android.database.DataSetObserver;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.SpinnerAdapter;
import android.widget.TextView;

import java.text.SimpleDateFormat;
import java.util.List;

import snhp.asu.edu.saladbar.Models.InterventionDay;
import snhp.asu.edu.saladbar.Models.ResearchTeamMember;
import snhp.asu.edu.saladbar.Models.School;

/**
 * Created by John on 9/6/17.
 */

public class CustomSpinnerAdapter implements SpinnerAdapter {

    private Context context;
    /**
     * The internal data (the ArrayList with the Objects).
     */
    private List<? extends Object> data;

    private SimpleDateFormat dateFormatter = new SimpleDateFormat("yyyy-MM-dd");

    public CustomSpinnerAdapter(Context context, List<? extends Object> data){
        this.context = context;
        this.data = data;
    }

    /**
     * Returns the Size of the ArrayList
     */
    @Override
    public int getCount() {
        return data.size();
    }

    @Override
    public Object getItem(int position) {
        return data.get(position);
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    @Override
    public int getItemViewType(int position) {
        return 0;
    }

    @Override
    public int getViewTypeCount() {
        return 1;
    }

    @Override
    public boolean hasStableIds() {
        return false;
    }

    @Override
    public boolean isEmpty() {
        return false;
    }

    @Override
    public View getDropDownView(int position, View convertView, ViewGroup parent) {
        if (convertView == null) {
            LayoutInflater vi = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            convertView = vi.inflate(R.layout.spinner_item, parent, false);
        }

        Object currentData = data.get(position);

        if(currentData instanceof School)
        {
            School school = (School) currentData;
            ((TextView) convertView).setText(school.getName());
        }

        else if(currentData instanceof InterventionDay)
        {
            // Making the initial one empty
            if(position == 0)
            {
                ((TextView) convertView).setText("");
            }
            else
            {
                InterventionDay interventionDay = (InterventionDay) currentData;
                String dateString = dateFormatter.format(interventionDay.getInterventionDate());
                ((TextView) convertView).setText(dateString);
            }
        }

        else if(currentData instanceof ResearchTeamMember)
        {
            ResearchTeamMember researchTeamMember = (ResearchTeamMember) currentData;
            String researchTeamMemberName = researchTeamMember.getFirstName() + " " + researchTeamMember.getLastName();
            ((TextView) convertView).setText(researchTeamMemberName);
        }

        else
        {
            throw new UnsupportedOperationException("The class " + data.getClass() + "'s spinner has not been implemented");
        }

        return convertView;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        //TextView textView = (TextView) View.inflate(context, android.R.layout.simple_spinner_item, null);
        TextView textView = (TextView) View.inflate(context, R.layout.spinner_item, null);
        textView.setTextAlignment(View.TEXT_ALIGNMENT_TEXT_END);
        Object currentData = data.get(position);

        if(currentData instanceof School)
        {
            School school = (School) currentData;
            textView.setText(school.getName());
        }

        else if(currentData instanceof  InterventionDay)
        {
            if(position == 0)
            {
                textView.setText("");
            }
            else
            {
                InterventionDay interventionDay = (InterventionDay) currentData;
                String dateString = dateFormatter.format(interventionDay.getInterventionDate());
                textView.setText(dateString);
            }
        }

        else if(currentData instanceof ResearchTeamMember)
        {
            ResearchTeamMember researchTeamMember = (ResearchTeamMember) currentData;
            String researchTeamMemberName = researchTeamMember.getFirstName() + " " + researchTeamMember.getLastName();
            textView.setText(researchTeamMemberName);
        }

        else
        {
            throw new UnsupportedOperationException("The class " + data.getClass() + "'s spinner has not been implemented");
        }


        return textView;
    }

    @Override
    public void registerDataSetObserver(DataSetObserver observer) {

    }

    @Override
    public void unregisterDataSetObserver(DataSetObserver observer) {

    }
}
