package snhp.asu.edu.saladbar.Models;

import com.google.gson.annotations.SerializedName;

import java.util.Date;

/**
 * Created by John on 9/28/17.
 */

public class Token {
    private int id;
    private String email;
    private String token;
    @SerializedName("expires")
    private Date tokenExpirationTime;

    // Audit fields
    private Date dtCreated;
    private String createdBy;
    private Date dtModified;
    private String modifiedBy;

    public Token(String email, String token, Date tokenExpirationTime) {
        this.email = email;
        this.token = token;
        this.tokenExpirationTime = tokenExpirationTime;
    }

    public Token(int id, String email, String token, Date tokenExpirationTime, Date dtCreated, String createdBy, Date dtModified, String modifiedBy) {
        this.id = id;
        this.email = email;
        this.token = token;
        this.tokenExpirationTime = tokenExpirationTime;
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

    public String getToken() {
        return token;
    }

    public void setToken(String token) {
        this.token = token;
    }

    public Date getTokenExpirationTime() {
        return tokenExpirationTime;
    }

    public void setTokenExpirationTime(Date tokenExpirationTime) {
        this.tokenExpirationTime = tokenExpirationTime;
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
