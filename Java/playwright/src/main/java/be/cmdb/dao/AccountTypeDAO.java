package be.cmdb.dao;

import java.util.List;

import org.hibernate.Session;
import org.hibernate.query.Query;

import be.cmdb.model.Type;

/**
* Data Access Object for AccountType entities.
*/
public class AccountTypeDAO implements BaseDAO<Type, Integer> {

    @Override
    public Type save(Session session, Type entity) {
        entity.setDiscriminator("accounttype");
        session.persist(entity);
        return entity;
    }

    @Override
    public Type update(Session session, Type entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public Type findById(Session session, Integer id) {
        return session.find(Type.class, id);
    }

    @Override
    public List<Type> findAll(Session session) {
        String hql = "FROM Type WHERE discriminator = 'accounttype'";
        Query<Type> query = session.createQuery(hql, Type.class);
        return query.getResultList();
    }

    @Override
    public void delete(Session session, Type entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, Integer id) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    @Override
    public long count(Session session) {
        String hql = "select count(*) from Type where discriminator = 'accounttype'";
        Query<Long> query = session.createQuery(hql, Long.class);
        return query.getSingleResult();
    }

    /**
     * Finds an IdentityType by its type.
     * @param session the Hibernate session
     * @param type the type to filter by
     * @return the IdentityType matching the type, or null if not found
     */
    public Type findByType(Session session, String type) {
        String hql = "FROM Type WHERE discriminator = 'accounttype' and type = :type";
        Query<Type> query = session.createQuery(hql, Type.class);
        query.setParameter("type", type);
        List<Type> results = query.getResultList();
        return results.isEmpty() ? null : results.get(0);
    }

    /**
     * Finds AccountTypes by the last modified admin ID.
     * @param session the Hibernate session
     * @param lastModifiedAdminId the ID of the admin who last modified the account type
     * @return a list of account types modified by the specified admin
     */
    public List<Type> findByLastModifiedAdminId(Session session, int lastModifiedAdminId) {
        String hql = "FROM Type WHERE discriminator = 'accounttype' and lastModifiedAdminId = :lastModifiedAdminId";
        Query<Type> query = session.createQuery(hql, Type.class);
        query.setParameter("lastModifiedAdminId", lastModifiedAdminId);
        return query.getResultList();
    }
}
