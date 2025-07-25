package be.cmdb.dao;

import java.util.List;

import org.hibernate.Session;
import org.hibernate.query.Query;

import be.cmdb.model.Type;

/**
* Data Access Object for IdentityType entities.
*/
public class IdentityTypeDAO implements BaseDAO<Type, Integer> {

    @Override
    public Type save(Session session, Type entity) {
        entity.setDiscriminator("identitytype");
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
        String hql = "FROM Type WHERE discriminator = 'identitytype'";
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
        String hql = "select count(*) from Type where discriminator = 'identitytype'";
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
        String hql = "from Type where discriminator = 'identitytype' and type = :type";
        Query<Type> query = session.createQuery(hql, Type.class);
        query.setParameter("type", type);
        List<Type> results = query.getResultList();
        return results.isEmpty() ? null : results.get(0);
    }

    public List<Type> findByLastModifiedAdminId(Session session, int lastModifiedAdminId) {
        String hql = "FROM Type WHERE discriminator = 'identitytype' and lastModifiedAdminId = :lastModifiedAdminId";
        Query<Type> query = session.createQuery(hql, Type.class);
        query.setParameter("lastModifiedAdminId", lastModifiedAdminId);
        return query.getResultList();
    }
}
