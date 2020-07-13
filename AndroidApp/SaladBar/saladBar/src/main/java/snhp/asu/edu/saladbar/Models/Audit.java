package snhp.asu.edu.saladbar.Models;

import java.util.Date;

/**
 * Created by John on 9/26/17.
 */

public class Audit {

    public static final String INSERT = "Insert";
    public static final String UPDATE = "Update";

    private int id;
    private String tableName;
    private String action;
    private String valueBefore;
    private String valueAfter;
    private String dirty;

    // Audit fields
    private Date dtCreated;
    private String createdBy;
    private Date dtModified;
    private String modifiedBy;

    public Audit(int id, String tableName, String action, String valueBefore, String valueAfter, String dirty, Date dtCreated, String createdBy, Date dtModified, String modifiedBy) {
        this.id = id;
        this.tableName = tableName;
        this.action = action;
        this.valueBefore = valueBefore;
        this.valueAfter = valueAfter;
        this.dirty = dirty;
        this.dtCreated = dtCreated;
        this.createdBy = createdBy;
        this.dtModified = dtModified;
        this.modifiedBy = modifiedBy;
    }

    public Audit(String tableName, String action, String valueBefore, String valueAfter, String createdBy, String modifiedBy) {
        this.tableName = tableName;
        this.action = action;
        this.valueBefore = valueBefore;
        this.valueAfter = valueAfter;
        this.dirty = "Y";
        this.createdBy = createdBy;
        this.modifiedBy = modifiedBy;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getTableName() {
        return tableName;
    }

    public void setTableName(String tableName) {
        this.tableName = tableName;
    }

    public String getAction() {
        return action;
    }

    public void setAction(String action) {
        this.action = action;
    }

    public String getValueBefore() {
        return valueBefore;
    }

    public void setValueBefore(String valueBefore) {
        this.valueBefore = valueBefore;
    }

    public String getValueAfter() {
        return valueAfter;
    }

    public void setValueAfter(String valueAfter) {
        this.valueAfter = valueAfter;
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
