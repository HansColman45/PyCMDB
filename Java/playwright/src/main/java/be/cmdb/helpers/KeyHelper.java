package be.cmdb.helpers;

import be.cmdb.dao.KeyDAO;
import be.cmdb.dao.LogDAO;
import be.cmdb.model.Key;
import be.cmdb.model.Log;
import org.hibernate.Session;
import org.hibernate.Transaction;

import java.util.List;

public class KeyHelper {

    public static void deleteKey(Session session, Key key){
        Transaction transaction = session.beginTransaction();
        KeyDAO keyDAO = new KeyDAO();
        LogDAO logDAO = new LogDAO();
        List<Log> logs = logDAO.findByKeyId(session, key.getKeyId());
        for (Log log : logs) {
            logDAO.delete(session, log);
        }
        keyDAO.delete(session, key);
        transaction.commit();
    }
}
