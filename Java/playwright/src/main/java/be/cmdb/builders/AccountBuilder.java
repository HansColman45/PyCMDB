package be.cmdb.builders;

import be.cmdb.model.Account;

/**
 * Builder for creating Account entities with default values.
 */
public class AccountBuilder extends GenericBuilder<Account> {
    private final Account account;

    /**
     * Constructor for AccountBuilder.
     */
    public AccountBuilder(){
        super();
        account = new Account();
        account.setUserId(getFaker().name().username());
        account.setActive(1); // Default to active
    }

    /**
     * Sets the userId for the account.
     * @param typeId string representing the user ID
     * @return AccountBuilder instance for method chaining
     */
    public AccountBuilder withTypeId(int typeId) {
        account.setTypeId(typeId);
        return this;
    }

    /**
     * Sets the userId for the account.
     * @param applicationId the ID of the application associated with the account
     * @return AccountBuilder instance for method chaining
     */
    public AccountBuilder withApplicationId(int applicationId) {
        account.setApplicationId(applicationId);
        return this;
    }

    /**
     * Sets the lastModifiedAdminId for the account.
     * @param lastModifiedAdminId the ID of the admin who last modified the account
     * @return AccountBuilder instance for method chaining
     */
    public AccountBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        account.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    /**
     * Sets if the account is active.
     * @param active integer representing the active status of the account (1 for active, 0 for inactive)
     * @return AccountBuilder instance for method chaining
     */
    public AccountBuilder withActive(int active) {
        account.setActive(active);
        return this;
    }

    /**
     * Returns the built Account instance.
     * @return Account instance
     */
    public Account build() {
        return account;
    }
}
