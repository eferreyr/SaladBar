package snhp.asu.edu.saladbar.Models;

import android.os.Parcel;
import android.os.Parcelable;

import java.util.Date;

/**
 * Created by John on 9/11/17.
 */

public class ResearchTeamMember implements Parcelable {
    private int id;
    private String email;
    private String firstName;
    private String lastName;
    private String active;

    // Audit fields
    private Date dtCreated;
    private String createdBy;
    private Date dtModified;
    private String modifiedBy;

    public ResearchTeamMember(int id, String email, String firstName, String lastName, String active) {
        this.id = id;
        this.email = email;
        this.firstName = firstName;
        this.lastName = lastName;
        this.active = active;
    }

    public ResearchTeamMember(int id, String email, String firstName, String lastName, String active, Date dtCreated, String createdBy, Date dtModified, String modifiedBy) {
        this.id = id;
        this.email = email;
        this.firstName = firstName;
        this.lastName = lastName;
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

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
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

    public ResearchTeamMember(Parcel in) {
        this.id = in.readInt();
        this.email = in.readString();
        this.firstName = in.readString();
        this.lastName = in.readString();
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
        out.writeString(this.email);
        out.writeString(this.firstName);
        out.writeString(this.lastName);
        out.writeString(this.active);

        out.writeLong(this.dtCreated.getTime());
        out.writeString(this.createdBy);
        out.writeLong(this.dtModified.getTime());
        out.writeString(this.modifiedBy);
    }


    public static final Parcelable.Creator<ResearchTeamMember> CREATOR = new Parcelable.Creator<ResearchTeamMember>() {
        public ResearchTeamMember createFromParcel(Parcel in) {
            return new ResearchTeamMember(in);
        }
        public ResearchTeamMember[] newArray(int size) {
            return new ResearchTeamMember[size];
        }
    };
}
