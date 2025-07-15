package be.cmdb.dao;

import be.cmdb.model.GeneralType;
import java.util.List;
import org.hibernate.Session;
import org.hibernate.query.Query;

/**
* Data Access Object for IdentityType entities.
*/
public class IdentityTypeDAO implements BaseDAO<GeneralType, Integer> {

    @Override
    public GeneralType save(Session session, GeneralType entity) {
        entity.setDiscriminator("identitytype");
        session.persist(entity);
        return entity;
    }

    @Override
    public GeneralType update(Session session, GeneralType entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public GeneralType findById(Session session, Integer id) {
        return session.find(GeneralType.class, id);
    }

    @Override
    public List<GeneralType> findAll(Session session) {
        String hql = "FROM type WHERE discriminator = 'identitytype'";
        Query<GeneralType> query = session.createQuery(hql, GeneralType.class);
        return query.getResultList();
    }

    @Override
    public void delete(Session session, GeneralType entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, Integer id) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    @Override
    public long count(Session session) {
        String hql = "select count(*) from type where discriminator = 'identitytype'";
        Query<Long> query = session.createQuery(hql, Long.class);
        return query.getSingleResult();
    }

    /**
     * Finds an IdentityType by its type.
     * @param session the Hibernate session
     * @param type the type to filter by
     * @return the IdentityType matching the type, or null if not found
     */
    public GeneralType findByType(Session session, String type) {
        String hql = "FROM type WHERE discriminator = 'identitytype' and type = :type";
        Query<GeneralType> query = session.createQuery(hql, GeneralType.class);
        query.setParameter("type", type);
        List<GeneralType> results = query.getResultList();
        return results.isEmpty() ? null : results.get(0);
    }
}
