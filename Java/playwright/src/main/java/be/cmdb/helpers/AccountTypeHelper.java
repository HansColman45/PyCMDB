package be.cmdb.helpers;

import be.cmdb.builders.AccountTypeBuilder;
import be.cmdb.builders.LogBuilder;
import be.cmdb.dao.AccountTypeDAO;
import be.cmdb.dao.LogDAO;
import be.cmdb.model.Admin;
import be.cmdb.model.Log;
import be.cmdb.model.Type;
import org.hibernate.Session;
import org.hibernate.Transaction;

import java.util.List;

public class AccountTypeHelper {

    /**
     * Creates a default AccountType object and persists it to the database using the provided session.
     * @param session The current Hibernate session
     * @param admin The Admin who is creating this AccountType
     * @param active Whether the AccountType should be active (1) or not (0)
     * @return Type object that has been persisted to the database
     */
    public static Type createDefaultAccountType(Session session, Admin admin, boolean active) {
        Transaction transaction = session.beginTransaction();
        AccountTypeDAO accountTypeDAO = new AccountTypeDAO();
        Type type = new AccountTypeBuilder()
            .withLastModifiedAdminId(admin.getAdminId())
            .withActive(active ? 1 : 0)
            .build();

        type = accountTypeDAO.save(session, type);
        // Log the creation of the AccountType
        String LogText = "The AccountType with Type: " + type.getType()
            + " and Description: " + type.getDescription()
            + " is created by Automation in table accounttype";
        Log log = new LogBuilder()
            .withType(type)
            .withLogText(LogText)
            .build();
        LogDAO logDAO = new LogDAO();
        logDAO.save(session, log);

        transaction.commit();
        return type;
    }

    /**
     * Creates a random AccountType object and persists it to the database using the provided session.
     * @return Type object
     */
    public static Type createRandomAccountType() {
        return new AccountTypeBuilder().build();
    }

    /**
     * Deletes an AccountType and all associated logs from the database.
     * @param session The current Hibernate session
     * @param type The Type object to be deleted
     */
    public static void deleteAccountType(Session session, Type type){
        Transaction transaction = session.beginTransaction();
        LogDAO logDAO = new LogDAO();
        List<Log> logs = logDAO.findByTypeId(session, type.getTypeId());
        for (Log log : logs) {
            logDAO.delete(session, log);
        }
        AccountTypeDAO accountTypeDAO = new AccountTypeDAO();
        accountTypeDAO.delete(session, type);
        transaction.commit();
    }
}
