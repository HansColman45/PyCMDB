package be.hans.cmdb.Actors;

import be.cmdb.helpers.IdentityTypeHelper;
import be.cmdb.helpers.LogLineHelper;
import be.cmdb.model.Admin;
import be.cmdb.model.Type;
import be.cmdb.pages.Type.CreateTypePage;
import be.cmdb.pages.Type.DeleteTypePage;
import be.cmdb.pages.Type.EditTypePage;
import be.hans.cmdb.Questions.IdentityType.OpenTheCreateIdentityTypePage;
import be.hans.cmdb.Questions.IdentityType.OpenTheDeleteIdentityTypePage;
import be.hans.cmdb.Questions.IdentityType.OpenTheEditIdentityTypePage;
import be.hans.cmdb.Tasks.ActivateTheIdentityType;

public class IdentityTypeActor extends CMDBActor{
    private final String table = "identitytype";

    /**
     * Constructor for CMDBActor.
     * @param name the name of the actor
     */
    public IdentityTypeActor(String name) {
        super(name);
    }

    /**
     * Creates a random identity type.
     * @return a random identity type
     */
    public Type createIdentityType(){
        return IdentityTypeHelper.createRandomIdentityType();
    }

    /**
     * Creates a default identity type with the specified admin and active status.
     * @param admin the admin creating the identity type
     * @param active whether the identity type is active
     * @return the created identity type
     */
    public Type createIdentityType(Admin admin, boolean active){
        return IdentityTypeHelper.createIdentityType(getSession(), admin, active);
    }

    /**
     * Automates the creation of the identityType.
     * @param type the identity type to be created
     */
    public void doCreateIdentityType(Type type){
        CreateTypePage createPage = asksFor(new OpenTheCreateIdentityTypePage());
        createPage.setType(type.getType());
        createPage.setDescription(type.getDescription());
        createPage.clickCreateButton();
        String value = "IdentityType with type: "+type.getType()+" and description: "+type.getDescription();
        setExpectedLogLine(LogLineHelper.createLogLine(value,getAccount().getUserId(),table));
    }

    /**
     * Automates the deletion of the identityType.
     * @param type the identity type to be deleted
     * @param reason the reason for deletion
     */
    public void doDeleteIdentityType(Type type, String reason){
        DeleteTypePage deletePage = asksFor(new OpenTheDeleteIdentityTypePage());
        deletePage.setReason(reason);
        deletePage.deActivate();
        String value = "IdentityType with type: "+type.getType()+" and description: "+type.getDescription();
        setExpectedLogLine(LogLineHelper.deleteLogLine(value,getAccount().getUserId(),reason,table));
    }

    /**
     * Automates the activation of the identityType.
     * @param type the identity type to be activated
     */
    public void doActivateTheIdentityType(Type type){
        asksFor(new ActivateTheIdentityType());
        String value = "IdentityType with type: "+type.getType()+" and description: "+type.getDescription();
        setExpectedLogLine(LogLineHelper.activeLogLine(value,getAccount().getUserId(),table));
    }

    /**
     * Automates the update of the identityType.
     * @param type the identity type to be updated
     * @param field the field to be updated
     * @param newValue the new value for the field
     * @return the updated identity type
     */
    public Type doUpdateIdentityType(Type type, String field, String newValue){
        String oldValue;
        newValue += getRandomInt();// Ensure newValue is unique for the test
        EditTypePage editTypePage = asksFor(new OpenTheEditIdentityTypePage());
        switch (field){
            case "type":
                oldValue = type.getType();
                type.setType(newValue);
                editTypePage.setType(newValue);
                setExpectedLogLine(LogLineHelper.updateLogLine(field,oldValue,newValue,getAccount().getUserId(),table));
                break;
            case "description":
                oldValue = type.getDescription();
                type.setDescription(newValue);
                editTypePage.setDescription(newValue);
                setExpectedLogLine(LogLineHelper.updateLogLine(field,oldValue,newValue,getAccount().getUserId(),table));
                break;
            default:
                throw new IllegalArgumentException("Field "+field+" is not supported");
        }
        editTypePage.edit();
        return type;
    }
}
