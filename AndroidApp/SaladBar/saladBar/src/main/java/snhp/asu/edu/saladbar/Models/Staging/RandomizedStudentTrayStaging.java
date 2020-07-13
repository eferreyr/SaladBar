package snhp.asu.edu.saladbar.Models.Staging;

import android.support.annotation.NonNull;

import java.util.Date;

import snhp.asu.edu.saladbar.Models.RandomizedStudentTray;

/**
 * Created by John on 2/20/18.
 */

public class RandomizedStudentTrayStaging {

    private long batchId;
    private String deviceId;
    private long randomizedStudentId;
    private long trayId;

    // Audit fields
    private Date dtCreated;
    private String createdBy;
    private Date dtModified;
    private String modifiedBy;

    public RandomizedStudentTrayStaging(long batchId, @NonNull String deviceId, @NonNull RandomizedStudentTray rst)
    {
        this.batchId = batchId;
        this.deviceId = deviceId;
        this.randomizedStudentId = rst.getRandomizedStudentId();
        this.trayId = rst.getTrayId();

        this.dtCreated = rst.getDtCreated();
        this.createdBy = rst.getCreatedBy();
        this.dtModified = rst.getDtModified();
        this.modifiedBy = rst.getModifiedBy();
    }

    // This method is for comparing the current object with the return object from the server.
    public boolean compareTo(@NonNull RandomizedStudentTrayStaging rst)
    {
        if (this.batchId != rst.batchId) return false;

        if (!this.deviceId.equalsIgnoreCase(rst.deviceId)) return false;

        if (this.randomizedStudentId != rst.randomizedStudentId) return false;

        if (this.trayId != rst.trayId) return false;

        if (!this.dtCreated.equals(rst.dtCreated)) return false;

        if (!this.createdBy.equalsIgnoreCase(rst.createdBy)) return false;

        if (!this.dtModified.equals(rst.dtModified)) return false;

        // Special logics for modifyBy
        if (this.modifiedBy == null && rst.modifiedBy == null) return true;

        // This line check to see if one of them is null but not the other.
        if (this.modifiedBy == null || rst.modifiedBy == null) return false;

        if (!this.modifiedBy.equalsIgnoreCase(rst.modifiedBy)) return false;

        return true;
    }
}
