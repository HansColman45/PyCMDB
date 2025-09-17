package be.hans.cmdb.Actors;

import be.hans.cmdb.Questions.Identity.OpenDeleteIdentityPage;
import be.hans.cmdb.Questions.Identity.OpenTheCreateIdentityPage;
import be.hans.cmdb.Questions.Identity.OpenTheEditIdentityPage;
import be.cmdb.helpers.IdentityHelper;
import be.cmdb.helpers.LogLineHelper;
import be.cmdb.model.Admin;
import be.cmdb.model.Identity;
import be.cmdb.pages.Identity.CreateIdentityPage;
import be.cmdb.pages.Identity.DeleteIdentityPage;
import be.cmdb.pages.Identity.EditIdentityPage;
import be.hans.cmdb.Tasks.ActivateIdentity;
import org.hibernate.Session;

/**
 * IdentityActor with specific abilities to manage identities in the CMDB application.
 */
public class IdentityActor extends CMDBActor {

    /**
     * Constructor for IdentityActor.
     * @param name the name of the actor
     */
    public IdentityActor(String name) {
        super(name);
    }

    /**
     * Creates a random Identity object with fake data.
     * @return Identity object
     */
    public Identity getIdentity(){
        return IdentityHelper.createRandomIdentity();
    }

    /**
     * Creates a random Identity object and persists it to the database using the provided session.
     * @param session The current hibernate session
     * @param admin The Admin who is creating this Identity
     * @param active Whether the Identity should be active (true) or not (false)
     * @return The persisted Identity
     */
    public Identity getIdentity(Session session, Admin admin, boolean active){
        return IdentityHelper.createRandomIdentity(session, admin, active);
    }

    /**
     * Automates the process of creating a new identity in the CMDB application.
     * Fills out the necessary fields and submits the form.
     * Also sets the expected log line for verification.
     * @param identity The Identity object containing the details to be filled in
     */
    public void doCreateIdentity(Identity identity)
    {
        CreateIdentityPage createPage = asksFor(new OpenTheCreateIdentityPage());
        createPage.setFirstName(identity.getFirstName());
        createPage.setLastName(identity.getLastName());
        createPage.setEmail(identity.getEmail());
        createPage.setCompany(identity.getCompany());
        createPage.setUserId(identity.getUserId());
        createPage.selectType("4");
        createPage.selectLanguage("English");
        createPage.create();
        String Value = "Identity with name: "+ identity.getName();
        String logLine = LogLineHelper.createLogLine(Value,getAccount().getUserId(), "identity");
        setExpectedLogLine(logLine);
    }

    /**
     * Automates the process of updating a specific field of an existing identity in the CMDB application.
     * Opens the edit page, modifies the specified field, and submits the changes.
     * Also sets the expected log line for verification.
     * @param identity The Identity object to be updated
     * @param field The field to be updated (e.g., "UserID", "FirstName", "LastName", "EMail")
     * @param newValue The new value to set for the specified field
     * @return The updated Identity object
     */
    public  Identity doUpdateIdenity(Identity identity, String field, String newValue){
        String oldValue = "";
        EditIdentityPage editPage = asksFor(new OpenTheEditIdentityPage());
        switch (field) {
            case "UserID":
                oldValue = identity.getUserId();
                identity.setUserId(newValue);
                editPage.setUserId(newValue);
                setExpectedLogLine(LogLineHelper.updateLogLine(field, oldValue, newValue,getAccount().getUserId(), "identity"));
                break;
            case "FirstName":
                oldValue = identity.getFirstName();
                identity.setFirstName(newValue);
                editPage.setFirstName(newValue);
                setExpectedLogLine(LogLineHelper.updateLogLine(field, oldValue, newValue,getAccount().getUserId(), "identity"));
                break;
            case "LastName":
                oldValue = identity.getLastName();
                identity.setLastName(newValue);
                editPage.setLastName(newValue);
                setExpectedLogLine(LogLineHelper.updateLogLine(field, oldValue, newValue,getAccount().getUserId(), "identity"));
                break;
            case "EMail":
                oldValue = identity.getEmail();
                identity.setEmail(newValue);
                editPage.setEmail(newValue);
                setExpectedLogLine(LogLineHelper.updateLogLine(field, oldValue, newValue,getAccount().getUserId(), "identity"));
                break;
            default:
                throw new IllegalArgumentException("Unknown field: " + field);
        }
        editPage.update();
        return identity;
    }

    /**
     * Automates the process of deactivating an existing identity in the CMDB application.
     * Opens the delete page, provides a reason for deactivation, and submits the request.
     * Also sets the expected log line for verification.
     * @param identity The Identity object to be deactivated
     * @param reason The reason for deactivation
     */
    public void doDeactivateIdentity(Identity identity, String reason){
        DeleteIdentityPage deletePage = asksFor(new OpenDeleteIdentityPage());
        deletePage.setReason(reason);
        deletePage.deActivate();
        String value = "Identity with name: "+ identity.getName()+", "+identity.getLastName();
        String logLine = LogLineHelper.deleteLogLine(value, getAccount().getUserId(), reason, "identity");
        setExpectedLogLine(logLine);
    }

    /**
     * Automates the process of activating an existing identity in the CMDB application.
     * Uses a task to perform the activation.
     * Also sets the expected log line for verification.
     * @param identity The Identity object to be activated
     */
    public void doActivateIdentity(Identity identity){
        String value = "Identity "+ identity.getName()+", "+identity.getLastName();
        setExpectedLogLine(LogLineHelper.activeLogLine(value, getAccount().getUserId(), "identity"));
        asksFor(new ActivateIdentity());
    }
}
