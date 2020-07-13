package snhp.asu.edu.saladbar.Models;

import android.os.Build;
import android.provider.Settings;

import java.util.Date;
import java.util.Locale;

import snhp.asu.edu.saladbar.SaladBarApp;

/**
 * Created by John on 9/26/17.
 */

public class Device {
    private int id;
    private String deviceId;
    private String deviceName;
    private String dirty;
    // Audit fields
    private Date dtCreated;
    private String createdBy;
    private Date dtModified;
    private String modifiedBy;

    public Device()
    {
        this.deviceId = Settings.Secure.getString(SaladBarApp.getContext().getContentResolver(), Settings.Secure.ANDROID_ID);
        this.deviceName = (Build.MANUFACTURER + " " + Build.MODEL).toUpperCase(Locale.getDefault());
        this.dirty = "Y";
    }

    public Device(int id, String deviceId, String deviceName, String dirty) {
        this.id = id;
        this.deviceId = deviceId;
        this.deviceName = deviceName;
        this.dirty = dirty;
    }

    public Device(int id, String deviceId, String deviceName, String dirty, Date dtCreated, String createdBy, Date dtModified, String modifiedBy) {
        this.id = id;
        this.deviceId = deviceId;
        this.deviceName = deviceName;
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

    public String getDeviceId() {
        return deviceId;
    }

    public void setDeviceId(String deviceId) {
        this.deviceId = deviceId;
    }

    public String getDeviceName() {
        return deviceName;
    }

    public void setDeviceName(String deviceName) {
        this.deviceName = deviceName;
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
