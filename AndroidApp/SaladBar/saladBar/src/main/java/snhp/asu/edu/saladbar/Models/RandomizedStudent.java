package snhp.asu.edu.saladbar.Models;

import android.os.Parcel;
import android.os.Parcelable;

import java.util.Date;

/**
 * Created by John on 8/2/17.
 */

public class RandomizedStudent implements Parcelable {
    private int id;
    private int schoolId;
    private int interventionDayId;
    private String studentId;
    private String gender;
    private String grade;
    private String assent; // 'Y' or 'N'
    private String dirty; // 'Y' or 'N'

    // Audit fields
    private Date dtCreated;
    private String createdBy;
    private Date dtModified;
    private String modifiedBy;

    public RandomizedStudent(int id, int schoolId, int interventionDayId, String studentId, String gender, String grade, String assent, String dirty) {
        this.id = id;
        this.schoolId = schoolId;
        this.interventionDayId = interventionDayId;
        this.studentId = studentId;
        this.gender = gender;
        this.grade = grade;
        this.assent = assent;
        this.dirty = dirty;
    }

    public RandomizedStudent(int id, int schoolId, int interventionDayId, String studentId, String gender, String grade, String assent) {
        this.id = id;
        this.schoolId = schoolId;
        this.interventionDayId = interventionDayId;
        this.studentId = studentId;
        this.gender = gender;
        this.grade = grade;
        this.assent = assent;
    }

    public RandomizedStudent(int id, int schoolId, int interventionDayId, String studentId, String gender, String grade, String assent, String dirty, Date dtCreated, String createdBy, Date dtModified, String modifiedBy) {
        this.id = id;
        this.schoolId = schoolId;
        this.interventionDayId = interventionDayId;
        this.studentId = studentId;
        this.gender = gender;
        this.grade = grade;
        this.assent = assent;
        this.dirty = dirty;
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

    public int getInterventionDayId() {
        return interventionDayId;
    }

    public void setInterventionDayId(int interventionDayId) {
        this.interventionDayId = interventionDayId;
    }

    public String getStudentId() {
        return studentId;
    }

    public void setStudentId(String studentId) {
        this.studentId = studentId;
    }

    public String getGender() {
        return gender;
    }

    public void setGender(String gender) {
        this.gender = gender;
    }

    public String getGrade() {
        return grade;
    }

    public void setGrade(String grade) {
        this.grade = grade;
    }

    public String getAssent() {
        return assent;
    }

    public void setAssent(String assent) {
        this.assent = assent;
    }

    public String getDirty() {
        return dirty;
    }

    public void setDirty(String dirty) {
        this.dirty = dirty;
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

    public RandomizedStudent (Parcel in) {
        this.id = in.readInt();
        this.schoolId = in.readInt();
        this.interventionDayId = in.readInt();
        this.studentId = in.readString();
        this.gender = in.readString();
        this.grade = in.readString();
        this.assent = in.readString();
        this.dirty = in.readString();

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
        out.writeInt(this.interventionDayId);
        out.writeString(this.studentId);
        out.writeString(this.gender);
        out.writeString(this.grade);
        out.writeString(this.assent);
        out.writeString(this.dirty);

        out.writeLong(this.dtCreated.getTime());
        out.writeString(this.createdBy);
        out.writeLong(this.dtModified.getTime());
        out.writeString(this.modifiedBy);
    }


    public static final Parcelable.Creator<RandomizedStudent> CREATOR = new Parcelable.Creator<RandomizedStudent>() {
        public RandomizedStudent createFromParcel(Parcel in) {
            return new RandomizedStudent(in);
        }
        public RandomizedStudent[] newArray(int size) {
            return new RandomizedStudent[size];
        }
    };
}
