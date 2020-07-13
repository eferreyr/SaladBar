package snhp.asu.edu.saladbar.Models;

import java.util.Date;

/**
 * Created by John on 9/18/17.
 */

public class RandomizedStudentTray {
    private int id;
    private long trayId;
    private int randomizedStudentId; // This is the row ID of Randomized Student
    private String dirty; // 'Y' or 'N'

    // Audit fields
    private Date dtCreated;
    private String createdBy;
    private Date dtModified;
    private String modifiedBy;

    public RandomizedStudentTray(int id, long trayId, int randomizedStudentId, String dirty) {
        this.id = id;
        this.trayId = trayId;
        this.randomizedStudentId = randomizedStudentId;
        this.dirty = dirty;
    }

    public RandomizedStudentTray(long trayId, int randomizedStudentId, String createdBy) {
        this.trayId = trayId;
        this.randomizedStudentId = randomizedStudentId;
        this.dirty = "Y";
        this.createdBy = createdBy;
    }

    public RandomizedStudentTray(int id, long trayId, int randomizedStudentId, String dirty, Date dtCreated, String createdBy, Date dtModified, String modifiedBy) {
        this.id = id;
        this.trayId = trayId;
        this.randomizedStudentId = randomizedStudentId;
        this.dirty = dirty;
        this.dtCreated = dtCreated;
        this.createdBy = createdBy;
        this.dtModified = dtModified;
        this.modifiedBy = modifiedBy;
    }

    @Override
    public boolean equals(Object object)
    {
        boolean isEqual= false;

        if (object != null && object instanceof RandomizedStudentTray)
        {
            isEqual = (this.trayId == ((RandomizedStudentTray) object).trayId);
        }

        return isEqual;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public long getTrayId() {
        return trayId;
    }

    public void setTrayId(long trayId) {
        this.trayId = trayId;
    }

    public int getRandomizedStudentId() {
        return randomizedStudentId;
    }

    public void setRandomizedStudentId(int randomizedStudentId) {
        this.randomizedStudentId = randomizedStudentId;
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
}
