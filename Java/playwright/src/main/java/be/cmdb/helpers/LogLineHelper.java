package be.cmdb.helpers;

public class LogLineHelper {

    /**
     * Create the logline when an objet is created in the application.
     * @param value the info of the created object
     * @param creator the user who created the object
     * @param table the table where the object is created
     * @return String the logline
     */
    public static String createLogLine(String value, String creator, String table) {
        return String.format("The %s is created by %s in table %s", value, creator, table);
    }

    /**
     * Create the logline when an objet is deleted in the application.
     * @param field The field that is updated
     * @param oldValue the old value of the field
     * @param newValue the new value of the field
     * @param editor the user who edited the object
     * @param table the table where the object is deleted
     * @return String the logline
     */
    public static String updateLogLine(String field, String oldValue, String newValue, String editor, String table) {
        return String.format("The %s has been changed from %s to %s by %s in table %s", field, oldValue, newValue, editor, table);
    }

    /**
     * Create the logline when an objet is deleted in the application.
     * @param value the info of the deleted object
     * @param deleter the user who deleted the object
     * @param reason the reason why the object is deleted
     * @param table the table where the object is deleted
     * @return String the logline
     */
    public static String deleteLogLine(String value, String deleter, String reason, String table) {
        return String.format("The %s is deleted due to %s by %s in table %s.", value, reason, deleter, table);
    }

    /**
     * Create the logline when an objet is activated in the application.
     * @param value the info of the deactivated object
     * @param activator the user who activated the object
     * @param table the table where the object is deactivated
     * @return String the logline
     */
    public static String activeLogLine(String value, String activator, String table) {
        return String.format("The %s is activated by %s in table %s", value, activator, table);
    }
}
