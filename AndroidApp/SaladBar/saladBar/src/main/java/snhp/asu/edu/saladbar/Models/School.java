package snhp.asu.edu.saladbar.Models;

import android.os.Parcel;
import android.os.Parcelable;

import java.util.Date;

/**
 * Created by John on 8/2/17.
 */

public class School implements Parcelable{

    public static int ELEMENTARY_SCHOOL = 1;
    public static int MIDDLE_SCHOOL = 2;
    public static int HIGH_SCHOOL = 3;

    private int id;
    private String name;
    private String district;
    private String mascot;
    private String colors;
    private byte[] schoolLogo;
    private int schoolTypeId;
    private String active;


    // Audit fields
    private Date dtCreated;
    private String createdBy;
    private Date dtModified;
    private String modifiedBy;

    public School(int id, String name, String district, String mascot, String colors, byte[] schoolLogo, int schoolTypeId, String active) {
        this.id = id;
        this.name = name;
        this.district = district;
        this.mascot = mascot;
        this.colors = colors;
        this.schoolLogo = schoolLogo;
        this.schoolTypeId = schoolTypeId;
        this.active = active;
    }

    public School(int id, String name, String district, String mascot, String colors, byte[] schoolLogo, int schoolTypeId, String active, Date dtCreated, String createdBy, Date dtModified, String modifiedBy) {
        this.id = id;
        this.name = name;
        this.district = district;
        this.mascot = mascot;
        this.colors = colors;
        this.schoolLogo = schoolLogo;
        this.schoolTypeId = schoolTypeId;
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

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getDistrict() {
        return district;
    }

    public void setDistrict(String district) {
        this.district = district;
    }

    public String getMascot() {
        return mascot;
    }

    public void setMascot(String mascot) {
        this.mascot = mascot;
    }

    public String getColors() {
        return colors;
    }

    public String getFirstColor()
    {
        String colorString = this.colors;

        // TODO: Make this better.
        if (colorString != null)
        {
            colorString = colorString.replace(" ", "");
            String[] colorStringSplit = colorString.split(",");
            if(colorStringSplit.length > 1)
            {
                return colorStringSplit[0];
            }

            else
            {
                return colorString;
            }
        }
        else
        {
            return "green";
        }
    }

    public void setColors(String colors) {
        this.colors = colors;
    }

    public byte[] getSchoolLogo() {
        return schoolLogo;
    }

    public void setSchoolLogo(byte[] schoolLogo) {
        this.schoolLogo = schoolLogo;
    }

    public int getSchoolTypeId() {
        return schoolTypeId;
    }

    public void setSchoolTypeId(int schoolTypeId) {
        this.schoolTypeId = schoolTypeId;
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

    public School (Parcel in) {
        this.id = in.readInt();
        this.name = in.readString();
        this.district = in.readString();
        this.mascot = in.readString();
        this.colors = in.readString();
        this.schoolLogo = new byte[in.readInt()];
        in.readByteArray(this.schoolLogo);
        this.schoolTypeId = in.readInt();
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
        out.writeString(this.name);
        out.writeString(this.district);
        out.writeString(this.mascot);
        out.writeString(this.colors);
        out.writeInt(this.schoolLogo.length);
        out.writeByteArray(this.schoolLogo);
        out.writeInt(this.schoolTypeId);
        out.writeString(this.active);

        out.writeLong(this.dtCreated.getTime());
        out.writeString(this.createdBy);
        out.writeLong(this.dtModified.getTime());
        out.writeString(this.modifiedBy);
    }

    public static final Parcelable.Creator<School> CREATOR = new Parcelable.Creator<School>() {
        public School createFromParcel(Parcel in) {
            return new School(in);
        }
        public School[] newArray(int size) {
            return new School[size];
        }
    };
}
