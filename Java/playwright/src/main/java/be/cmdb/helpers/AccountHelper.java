package be.cmdb.helpers;

import java.util.List;

import org.hibernate.Session;
import org.hibernate.Transaction;

import be.cmdb.builders.AccountBuilder;
import be.cmdb.builders.LogBuilder;
import be.cmdb.dao.AccountDAO;
import be.cmdb.dao.AccountTypeDAO;
import be.cmdb.dao.ApplicationDAO;
import be.cmdb.dao.LogDAO;
import be.cmdb.model.Account;
import be.cmdb.model.Admin;
import be.cmdb.model.Application;
import be.cmdb.model.Log;
import be.cmdb.model.Type;

/**
 * Helper class for creating and managing Account entities.
 */
public final class AccountHelper {

    /**
     * Creates a simple Account with default values.
     * @param session the Hibernate session
     * @param admin the Admin who is creating the account
     * @param active boolean indicating if the account is active
     * @return the persisted Account entity
     */
    public Account createSimpleAccount(Session session, Admin admin, boolean active) {
        AccountTypeDAO accountTypeDAO = new AccountTypeDAO();
        Type accountType = accountTypeDAO.findByType(session,"Normal User");

        ApplicationDAO applicationDAO = new ApplicationDAO();
        Application application = applicationDAO.findByName(session, "CMDB").get(0);

        Account account = new AccountBuilder()
            .withActive(active ? 1 : 0)
            .withLastModifiedAdminId(admin.getAdminId())
            .withTypeId(accountType)
            .withApplicationId(application.getAppId())
            .build();
        AccountDAO accountDAO = new AccountDAO();
        account =  accountDAO.save(session, account);

        String LogText = "The Account with UserID: " + account.getUserId()
            +" with type "+ accountType.getType()
            +" for application "+ application.getName()+ " is created by Automation in table account";
        LogDAO logDAO = new LogDAO();
        Log log = new LogBuilder()
            .withAccount(account)
            .withLogText(LogText)
            .build();
        logDAO.save(session, log);

        return account;
    }

    /**
     * Creates a random Account without persisting it.
     * @param session the Hibernate session
     * @return the Account entity
     */
    public static Account createRandomAccount(Session session) {
        AccountTypeDAO accountTypeDAO = new AccountTypeDAO();
        Type accountType = accountTypeDAO.findByType(session,"Normal User");

        ApplicationDAO applicationDAO = new ApplicationDAO();
        Application application = applicationDAO.findByName(session, "CMDB").get(0);

        return new AccountBuilder()
            .withApplicationId(application.getAppId())
            .withTypeId(accountType)
            .build();
    }

    /**
     * Deletes an Account and all associated logs.
     * @param session the Hibernate session
     * @param account the account to be deleted
     */
    public static void delete(Session session, Account account) {
        Transaction transaction = session.beginTransaction();
        AccountDAO accountDAO = new AccountDAO();
        LogDAO logDAO = new LogDAO();
        List<Log> logs = logDAO.findByAccountId(session, account.getAccId());
        for (Log log : logs) {
            logDAO.delete(session, log);
        }
        accountDAO.delete(session, account);
        transaction.commit();
    }
}
