package be.cmdb.model;

import java.time.LocalDateTime;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;

@Entity
public class IdenAccount {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    private LocalDateTime validFrom;
    private LocalDateTime validUntil;
    private int lastModifiedAdminId;
    private int accountId;
    private int identityId;

    /**
     * Gets the unique identifier of the IdenAccount.
     * @return the ID
     */
    public int getId() {
        return id;
    }

    /**
     * Sets the unique identifier of the IdenAccount.
     * @param id the ID to set
     */
    public void setId(int id) {
        this.id = id;
    }

    /**
     * Gets the start date and time when the account is valid.
     * @return the valid from date
     */
    public LocalDateTime getValidFrom() {
        return validFrom;
    }

    /**
     * Sets the start date and time when the account is valid.
     * @param validFrom the valid from date to set
     */
    public void setValidFrom(LocalDateTime validFrom) {
        this.validFrom = validFrom;
    }

    /**
     * Gets the end date and time when the account is valid.
     * @return the valid until date
     */
    public LocalDateTime getValidUntil() {
        return validUntil;
    }

    /**
     * Sets the end date and time when the account is valid.
     * @param validUntil the valid until date to set
     */
    public void setValidUntil(LocalDateTime validUntil) {
        this.validUntil = validUntil;
    }

    /**
     * Gets the ID of the last admin who modified this account.
     * @return the last modified admin ID
     */
    public int getLastModifiedAdminId() {
        return lastModifiedAdminId;
    }

    /**
     * Sets the ID of the last admin who modified this account.
     * @param lastModifiedAdminId the last modified admin ID to set
     */
    public void setLastModifiedAdminId(int lastModifiedAdminId) {
        this.lastModifiedAdminId = lastModifiedAdminId;
    }

    /**
     * Gets the account ID associated with this IdenAccount.
     * @return the account ID
     */
    public int getAccountId() {
        return accountId;
    }

    /**
     * Sets the account ID associated with this IdenAccount.
     * @param accountId the account ID to set
     */
    public void setAccountId(int accountId) {
        this.accountId = accountId;
    }

    /**
     * Gets the identity ID associated with this IdenAccount.
     * @return the identity ID
     */
    public int getIdentityId() {
        return identityId;
    }

    /**
     * Sets the identity ID associated with this IdenAccount.
     * @param identityId the identity ID to set
     */
    public void setIdentityId(int identityId) {
        this.identityId = identityId;
    }
}
