package be.cmdb.helpers;

import be.cmdb.dao.AccountTypeDAO;
import be.cmdb.dao.LogDAO;
import be.cmdb.model.Log;
import be.cmdb.model.Type;
import org.hibernate.Session;
import org.hibernate.Transaction;

import java.util.List;

public class AccountTypeHelper {

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
