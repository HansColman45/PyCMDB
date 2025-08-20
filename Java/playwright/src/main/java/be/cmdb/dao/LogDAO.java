package be.cmdb.dao;

import be.cmdb.model.Log;
import java.util.List;
import org.hibernate.JDBCException;
import org.hibernate.Session;
import org.hibernate.query.Query;

/**
* Data Access Object for Log entity operations.
* Provides CRUD operations and specialized queries for Log entities.
* All operations use the provided Hibernate Session for database access.
* Transaction management should be handled externally.
*/
public class LogDAO implements BaseDAO<Log, Integer> {

    /**
     * Default constructor.
     */
    public LogDAO() {
    }

    /**
     * Saves a new log entry to the database using the provided session.
     * @param session the Hibernate session to use
     * @param log the Log entity to save
     * @return the saved Log entity with generated ID
     */
    @Override
    public Log save(Session session, Log log) {
        try {
            session.persist(log);
            return log;
        } catch (JDBCException e) {
            throw new RuntimeException("Error saving log: " + e.getMessage(), e);
        }
    }

    /**
     * Updates an existing log entry in the database using the provided session.
     * @param session the Hibernate session to use
     * @param log the Log entity to update
     * @return the updated Log entity
     */
    @Override
    public Log update(Session session, Log log) {
        try {
            return session.merge(log);
        } catch (JDBCException e) {
            throw new RuntimeException("Error updating log: " + e.getMessage(), e);
        }
    }

    /**
     * Finds a log entry by its ID using the provided session.
     * @param session the Hibernate session to use
     * @param id the ID of the log entry
     * @return the Log entity if found, null otherwise
     */
    @Override
    public Log findById(Session session, Integer id) {
        try {
            return session.find(Log.class, id);
        } catch (JDBCException e) {
            throw new RuntimeException("Error finding log by ID: " + e.getMessage(), e);
        }
    }

    /**
     * Retrieves all log entries from the database using the provided session.
     * @param session the Hibernate session to use
     * @return list of all Log entities
     */
    @Override
    public List<Log> findAll(Session session) {
        try {
            Query<Log> query = session.createQuery("FROM Log", Log.class);
            return query.getResultList();
        } catch (JDBCException e) {
            throw new RuntimeException("Error retrieving all logs: " + e.getMessage(), e);
        }
    }

    /**
     * Deletes a log entry from the database using the provided session.
     * @param session the Hibernate session to use
     * @param log the Log entity to delete
     */
    @Override
    public void delete(Session session, Log log) {
        try {
            session.remove(log);
        } catch (JDBCException e) {
            throw new RuntimeException("Error deleting log: " + e.getMessage(), e);
        }
    }

    /**
     * Deletes a log entry by its ID using the provided session.
     * @param session the Hibernate session to use
     * @param id the ID of the log entry to delete
     */
    @Override
    public void deleteById(Session session, Integer id) {
        try {
            Log log = session.find(Log.class, id);
            if (log != null) {
                session.remove(log);
            }
        } catch (JDBCException e) {
            throw new RuntimeException("Error deleting log by ID: " + e.getMessage(), e);
        }
    }

    /**
     * Counts the total number of log entries using the provided session.
     * @param session the Hibernate session to use
     * @return the total count of log entries
     */
    @Override
    public long count(Session session) {
        try {
            Query<Long> query = session.createQuery("SELECT COUNT(*) FROM Log", Long.class);
            return query.getSingleResult();
        } catch (JDBCException e) {
            throw new RuntimeException("Error counting logs: " + e.getMessage(), e);
        }
    }

    /**
     * Finds log entries by identity ID using the provided session.
     * @param session the Hibernate session to use
     * @param identityId the identity ID to search for
     * @return list of Log entities associated with the identity
     */
    public List<Log> findByIdentityId(Session session, Integer identityId) {
        String hql = "select {l.*}, {i.*} FROM Log l join \"Identity\" i on i.idenId = l.identityId " +
            " where l.identityId = :identityId";
        Query<Log> query = session.createNativeQuery(hql, Log.class)
            .setParameter("identityId", identityId)
            .addEntity("l",Log.class)
            .addJoin("i", "l.identityId");

        return query.getResultList();
    }

    /**
     * Finds log entries by account ID using the provided session.
     * @param session the Hibernate session to use
     * @param accountId the account ID to search for
     * @return list of Log entities associated with the account
     */
    public List<Log> findByAccountId(Session session, int accountId) {
        String hql = "select {l.*}, {a.*} FROM Log l join Account a on a.accId = l.accountId " +
            " where l.accountId = :accountId ORDER BY l.logDate DESC";
        Query<Log> query = session.createNativeQuery(hql, Log.class)
            .setParameter("accountId", accountId)
            .addEntity("l", Log.class)
            .addJoin("a", "l.accountId");
        return query.getResultList();
    }

    /**
     * Finds log entries by key ID using the provided session.
     * @param session the Hibernate session to use
     * @param keyId the key ID to search for
     * @return list of Log entities associated with the key
     */
    public List<Log> findByKensingtonId(Session session, int keyId) {
        String hql = "select {l.*}, {k.*} FROM Log l join Kensington k on k.keyId = l.kensingtonId " +
            " where l.kensingtonId = :keyId ORDER BY l.logDate DESC";
        Query<Log> query = session.createNativeQuery(hql, Log.class)
            .setParameter("keyId", keyId)
            .addEntity("l", Log.class)
            .addJoin("k", "l.kensingtonId");
        return query.getResultList();
    }

    /**
     * Finds log entries by mobile ID using the provided session.
     * @param session the Hibernate session to use
     * @param mobileId the mobile ID to search for
     * @return list of Log entities associated with the mobile
     */
    public List<Log> findByMobileId(Session session, int mobileId) {
        String hql = "select {l.*}, {m.*} FROM Log l join Mobile m on m.mobileId = l.mobileId " +
            " where l.mobileId = :mobileId ORDER BY l.logDate DESC";
        Query<Log> query = session.createNativeQuery(hql, Log.class)
            .setParameter("mobileId", mobileId)
            .addEntity("l", Log.class)
            .addJoin("m", "l.mobileId");
        return query.getResultList();
    }

    /**
     * Finds log entries by type ID using the provided session.
     * @param session the Hibernate session to use
     * @param typeId the type ID to search for
     * @return list of Log entities associated with the type
     */
    public List<Log> findByTypeId(Session session, int typeId) {
        String hql = "select {l.*}, {a.*} FROM Log l join Type a on a.typeId = l.typeId " +
            " where l.typeId = :typeId ORDER BY l.logDate DESC";
        Query<Log> query = session.createNativeQuery(hql, Log.class)
            .setParameter("typeId", typeId)
            .addEntity("l", Log.class)
            .addJoin("a", "l.typeId");
        return query.getResultList();
    }

    /**
     * Finds log entries by asset type ID using the provided session.
     * @param session the Hibernate session to use
     * @param typeId the asset type ID to search for
     * @return list of Log entities associated with the asset type
     */
    public List<Log> findByAssetTypeId(Session session, int typeId) {
        String hql = "select {l.*}, {a.*} FROM Log l join AssetType a on a.TypeId = l.assetTypeId " +
            " where l.assetTypeId = :typeId ORDER BY l.logDate DESC";
        Query<Log> query = session.createNativeQuery(hql, Log.class)
            .setParameter("typeId", typeId)
            .addEntity("l", Log.class)
            .addJoin("a", "l.assetTypeId");
        return query.getResultList();
    }

    /**
     * Finds log entries by assetTag using the provided session.
     * @param session the Hibernate session to use
     * @param assetTag the asset tag to search for
     * @return list of Log entities associated with the asset tag
     */
    public List<Log> findByAssetTag(Session session, String assetTag) {
        String hql = "select {l.*}, {a.*} FROM Log l join Asset a on a.assetTag = l.assetTag " +
            " where l.assetTag = :assetTag ORDER BY l.logDate DESC";
        Query<Log> query = session.createNativeQuery(hql, Log.class)
            .setParameter("assetTag", assetTag)
            .addEntity("l", Log.class)
            .addJoin("a", "l.assetTag");
        return query.getResultList();
    }

    /**
     * Finds log entries by admin ID using the provided session.
     * @param session the Hibernate session to use
     * @param adminId the admin ID to search for
     * @return list of Log entities associated with the admin
     */
    public List<Log> findByAdminId(Session session, int adminId) {
        String hql = "select {l.*}, {a.*} FROM Log l join Admin a on l.adminId = a.admin_Id " +
            " where l.adminId = :adminId ORDER BY l.logDate DESC";
        Query<Log> query = session.createNativeQuery(hql, Log.class)
            .setParameter("adminId", adminId)
            .addEntity("l", Log.class)
            .addJoin("a", "l.adminId");
        return query.getResultList();
    }
}
