package be.cmdb.helpers;

import be.cmdb.dao.IdentityTypeDAO;
import be.cmdb.dao.LogDAO;
import be.cmdb.model.Admin;
import be.cmdb.model.Log;
import be.cmdb.model.Type;
import org.hibernate.Session;
import org.hibernate.Transaction;
import java.util.List;

public class IdentityTypeHelper {

    public static Type createIdentityType(Session session, Admin admin){
        return new Type();
    }

    public static void deleteType(Session session, Type type){
        Transaction transaction = session.beginTransaction();
        LogDAO logDAO = new LogDAO();
        List<Log> logs = logDAO.findByTypeId(session, type.getTypeId());
        for (Log log : logs) {
            logDAO.delete(session, log);
        }
        IdentityTypeDAO identityDAO = new IdentityTypeDAO();
        identityDAO.delete(session, type);
        transaction.commit();
    }
}
