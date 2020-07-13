package snhp.asu.edu.saladbar.Models.Staging;

import android.support.annotation.NonNull;

import java.util.Date;

import snhp.asu.edu.saladbar.Models.RandomizedStudent;

/**
 * Created by John on 2/5/18.
 */

public class RandomizedStudentStaging {

    private long batchId;
    private String deviceId;
    private long randomizedStudentId;
    private long interventionDayId;
    private String studentId;
    private String assent; // 'Y' or 'N'

    // Audit fields
    private Date dtCreated;
    private String createdBy;
    private Date dtModified;
    private String modifiedBy;

    public RandomizedStudentStaging(long batchId, @NonNull String deviceId, @NonNull RandomizedStudent rs)
    {
        this.batchId = batchId;
        this.deviceId = deviceId;
        this.randomizedStudentId = rs.getId();
        this.interventionDayId = rs.getInterventionDayId();
        this.studentId = rs.getStudentId();
        this.assent = rs.getAssent();

        this.dtCreated = rs.getDtCreated();
        this.createdBy = rs.getCreatedBy();
        this.dtModified = rs.getDtModified();
        this.modifiedBy = rs.getModifiedBy();
    }

    // This method is for comparing the current object with the return object from the server.
    public boolean compareTo(@NonNull RandomizedStudentStaging rss)
    {
        if (this.batchId != rss.batchId) return false;

        if (!this.deviceId.equalsIgnoreCase(rss.deviceId)) return false;

        if (this.randomizedStudentId != rss.randomizedStudentId) return false;

        if (this.interventionDayId != rss.interventionDayId) return false;

        if (!this.studentId.equalsIgnoreCase(rss.studentId)) return false;

        if (!this.assent.equalsIgnoreCase(rss.assent)) return false;

        if (!this.dtCreated.equals(rss.dtCreated)) return false;

        if (!this.createdBy.equalsIgnoreCase(rss.createdBy)) return false;

        if (!this.dtModified.equals(rss.dtModified)) return false;

        if (!this.modifiedBy.equalsIgnoreCase(rss.modifiedBy)) return false;


        return true;
    }
}
