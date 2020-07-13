package snhp.asu.edu.saladbar.Models;

import android.os.Parcel;
import android.os.Parcelable;

import com.google.gson.annotations.SerializedName;

import java.util.Date;

/**
 * Created by John on 8/14/17.
 */

public class InterventionDay implements Parcelable {
    private int id;
    private int schoolId;
    @SerializedName("dtIntervention")
    private Date interventionDate;
    private String interventionFinished;
    private String active;

    // Audit fields
    private Date dtCreated;
    private String createdBy;
    private Date dtModified;
    private String modifiedBy;

    public InterventionDay(int id, int schoolId, Date interventionDate, String interventionFinished, String active) {
        this.id = id;
        this.schoolId = schoolId;
        this.interventionDate = interventionDate;
        this.interventionFinished = interventionFinished;
        this.active = active;
    }

    public InterventionDay(int id, int schoolId, Date interventionDate, String interventionFinished, String active, Date dtCreated, String createdBy, Date dtModified, String modifiedBy) {
        this.id = id;
        this.schoolId = schoolId;
        this.interventionDate = interventionDate;
        this.interventionFinished = interventionFinished;
        this.active = active;
        this.dtCreated = dtCreated;
        this.createdBy = createdBy;
        this.dtModified = dtModified;
        this.modifiedBy = modifiedBy;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getSchoolId() {
        return schoolId;
    }

    public void setSchoolId(int schoolId) {
        this.schoolId = schoolId;
    }

    public Date getInterventionDate() {
        return interventionDate;
    }

    public void setInterventionDate(Date interventionDate) {
        this.interventionDate = interventionDate;
    }

    public String getInterventionFinished() {
        return interventionFinished;
    }

    public void setInterventionFinished(String interventionFinished) {
        this.interventionFinished = interventionFinished;
    }

    public String getActive() {
        return active;
    }

    public void setActive(String active) {
        this.active = active;
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

    public Date getDtModified() {
        return dtModified;
    }

    public void setDtModified(Date dtModified) {
        this.dtModified = dtModified;
    }

    public String getModifiedBy() {
        return modifiedBy;
    }

    public void setModifiedBy(String modifiedBy) {
        this.modifiedBy = modifiedBy;
    }

    public InterventionDay (Parcel in) {
        this.id = in.readInt();
        this.schoolId = in.readInt();
        this.interventionDate = new Date(in.readLong());
        this.interventionFinished = in.readString();
        this.active = in.readString();

        this.dtCreated = new Date(in.readLong());
        this.createdBy = in.readString();
        this.dtModified = new Date(in.readLong());
        this.modifiedBy = in.readString();
    }

    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel out, int flags) {
        out.writeInt(this.id);
        out.writeInt(this.schoolId);
        out.writeLong(this.interventionDate.getTime());
        out.writeString(this.interventionFinished);
        out.writeString(this.active);

        out.writeLong(this.dtCreated.getTime());
        out.writeString(this.createdBy);
        out.writeLong(this.dtModified.getTime());
        out.writeString(this.modifiedBy);
    }

    public static final Parcelable.Creator<InterventionDay> CREATOR = new Parcelable.Creator<InterventionDay>() {
        public InterventionDay createFromParcel(Parcel in) {
            return new InterventionDay(in);
        }
        public InterventionDay[] newArray(int size) {
            return new InterventionDay[size];
        }
    };
}
