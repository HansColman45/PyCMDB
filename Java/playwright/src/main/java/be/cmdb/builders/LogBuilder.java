package be.cmdb.builders;

import be.cmdb.model.Log;
import java.time.LocalDateTime;
import java.time.ZoneOffset;

/**
 * Builder for creating Log entities with default values.
 */
public class LogBuilder extends GenericBuilder {
    private final Log log;

    /**
     * Constructor for LogBuilder.
     */
    public LogBuilder() {
        super();
        log = new Log();
        log.setLogDate(LocalDateTime.now(ZoneOffset.UTC));
    }

    /**
     * Sets the LogText for the log entry.
     * @param logText string representing the log text
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withLogText(String logText) {
        log.setLogText(logText);
        return this;
    }

    /**
     * Sets the accountId for the log entry.
     * @param accountId the ID of the account associated with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withAccountId(int accountId) {
        log.setAccountId(accountId);
        return this;
    }

    /**
     * Sets the typeId for the log entry.
     * @param typeId the ID of the type associated with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withTypeId(int typeId) {
        log.setTypeId(typeId);
        return this;
    }

    /**
     * Sets the assetTag for the log entry.
     * @param assetTag the asset tag associated with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withAssetTag(String assetTag) {
        log.setAssetTag(assetTag);
        return this;
    }

    /**
     * Sets the identityId for the log entry.
     * @param identityId the ID of the identity associated with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withIdentityId(int identityId) {
        log.setIdentityId(identityId);
        return this;
    }

    /**
     * Builds the log entry.
     * @return Log instance
     */
    public Log build(){
        return log;
    }
}
