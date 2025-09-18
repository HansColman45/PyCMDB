package be.hans.cmdb.Actors;

import be.cmdb.helpers.AccountTypeHelper;
import be.cmdb.helpers.LogLineHelper;
import be.cmdb.model.Admin;
import be.cmdb.model.Type;
import be.cmdb.pages.Type.CreateTypePage;
import be.cmdb.pages.Type.DeleteTypePage;
import be.cmdb.pages.Type.EditTypePage;
import be.hans.cmdb.Questions.AccountType.OpenTheCreateAccountTypePage;
import be.hans.cmdb.Questions.AccountType.OpenTheDeleteAccountTypePage;
import be.hans.cmdb.Questions.AccountType.OpenTheEditAccountTypePage;
import be.hans.cmdb.Tasks.ActivateTheAccountType;

/**
 * Actor representing an account type in the CMDB application.
 */
public class AccountTypeActor extends CMDBActor {
    private final String table = "accounttype";
    /**
     * Constructor for CMDBActor.
     * @param name the name of the actor
     */
    public AccountTypeActor(String name) {
        super(name);
    }

    /**
     * Creates a random account type.
     * @return a random account type
     */
    public Type createAccountType(){
        return AccountTypeHelper.createRandomAccountType();
    }

    /**
     * Creates a default account type with the specified admin and active status.
     * @param admin the admin creating the account type
     * @param active whether the account type is active
     * @return the created account type
     */
    public Type createAccountType(Admin admin, boolean active){
        return AccountTypeHelper.createDefaultAccountType(getSession(), admin, active);
    }

    /**
     * Automates the creation of the accountType.
     * @param type the account type to be created
     */
    public void doCreateAccountType(Type type){
        CreateTypePage createPage = asksFor(new OpenTheCreateAccountTypePage());
        createPage.setType(type.getType());
        createPage.setDescription(type.getDescription());
        createPage.clickCreateButton();
        String value = "AccountType with type: "+type.getType()+" and description: "+type.getDescription();
        setExpectedLogLine(LogLineHelper.createLogLine(value,getAccount().getUserId(),table));
    }

    /**
     * Automates the deletion of the accountType.
     * @param type the account type to be deleted
     * @param reason the reason for deletion
     */
    public void doDeleteAccountType(Type type, String reason){
        DeleteTypePage deletePage = asksFor(new OpenTheDeleteAccountTypePage());
        deletePage.setReason(reason);
        deletePage.deActivate();
        String value = "accounttype with type: "+type.getType()+" and description: "+type.getDescription();
        setExpectedLogLine(LogLineHelper.deleteLogLine(value,getAccount().getUserId(),reason,table));
    }

    /**
     * Automates the activation of the accountType.
     * @param type the account type to be activated
     */
    public void doActivateAccountType(Type type){
        String value = "accounttype with type: "+type.getType()+" and description: "+type.getDescription();
        setExpectedLogLine(LogLineHelper.activeLogLine(value,getAccount().getUserId(),table));
        asksFor(new ActivateTheAccountType());
    }

    /**
     * Automates the update of a specific field in the account type.
     * @param type the account type to be updated
     * @param field the field to be updated (e.g., "type", "description")
     * @param newValue the new value for the specified field
     * @return the updated account type
     */
    public Type doUpdateAccountType(Type type, String field, String newValue){
        String oldValue;
        EditTypePage editTypePage = asksFor(new OpenTheEditAccountTypePage());
        // Update the specified field
        switch (field) {
            case "type":
                oldValue = type.getType();
                editTypePage.setType(newValue);
                type.setType(newValue);
                setExpectedLogLine(LogLineHelper.updateLogLine(field, oldValue, newValue, getAccount().getUserId(), table));
                break;
            case "description":
                oldValue = type.getDescription();
                editTypePage.setDescription(newValue);
                type.setDescription(newValue);
                setExpectedLogLine(LogLineHelper.updateLogLine(field, oldValue, newValue, getAccount().getUserId(), table));
                break;
            default:
                throw new IllegalArgumentException("Unknown field: " + field);
        }
        editTypePage.edit();
        return type;
    }
}
