package snhp.asu.edu.saladbar.Models;

import java.util.Date;

/**
 * Created by John on 10/3/17.
 */

public class Tracking {

    public static final String ASSENT = "assent";
    public static final String TRAY = "tray";

    private int id;
    private String input;
    private String location;
    private String school;
    private Date interventionDay;

    // Audit fields
    private Date dtCreated;
    private String createdBy;

    public Tracking(String input, String location, String school, Date interventionDay, String createdBy) {
        this.input = input;
        this.location = location;
        this.school = school;
        this.interventionDay = interventionDay;
        this.createdBy = createdBy;
    }

    public Tracking(int id, String input, String location, String school, Date interventionDay, Date dtCreated, String createdBy) {
        this.id = id;
        this.input = input;
        this.location = location;
        this.school = school;
        this.interventionDay = interventionDay;
        this.dtCreated = dtCreated;
        this.createdBy = createdBy;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getInput() {
        return input;
    }

    public void setInput(String input) {
        this.input = input;
    }

    public String getLocation() {
        return location;
    }

    public void setLocation(String location) {
        this.location = location;
    }

    public String getSchool() {
        return school;
    }

    public void setSchool(String school) {
        this.school = school;
    }

    public Date getInterventionDay() {
        return interventionDay;
    }

    public void setInterventionDay(Date interventionDay) {
        this.interventionDay = interventionDay;
    }

    public Date getDtCreated() {
        return dtCreated;
    }

    public void setDtCreated(Date dtCreated) {
        this.dtCreated = dtCreated;
    }

    public String getCreatedBy() {
        return createdBy;
    }

    public void setCreatedBy(String createdBy) {
        this.createdBy = createdBy;
    }
}
