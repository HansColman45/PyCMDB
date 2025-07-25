package be.cmdb.dao;
import be.cmdb.model.SubscriptionType;
import org.hibernate.Session;
import org.hibernate.query.Query;

import java.util.Collections;
import java.util.List;

public class SubscriptionTypeDAO implements BaseDAO<SubscriptionType, Integer> {
    @Override
    public SubscriptionType save(Session session, SubscriptionType entity) {
        session.persist(entity);
        return entity;
    }

    @Override
    public SubscriptionType update(Session session, SubscriptionType entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public SubscriptionType findById(Session session, Integer id) {
        return session.find(SubscriptionType.class, id);
    }

    @Override
    public List<SubscriptionType> findAll(Session session) {
        return Collections.emptyList();
    }

    @Override
    public void delete(Session session, SubscriptionType entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, Integer id) {
        SubscriptionType subscriptionType = findById(session, id);
        if(subscriptionType != null) {
            delete(session, subscriptionType);
        } else {
            // Optionally handle the case where the subscription type is not found
            System.out.println("SubscriptionType with ID " + id + " not found.");
        }
    }

    @Override
    public long count(Session session) {
        Query<Long> query = session.createQuery("SELECT COUNT(s) FROM SubscriptionType s", Long.class);
        return query.getSingleResult();
    }

    /**
     * Finds subscription types by the last modified admin ID.
     * @param session the Hibernate session
     * @param adminId the ID of the admin who last modified the subscription type
     * @return a list of subscription types modified by the specified admin
     */
    public List<SubscriptionType> findByLastModifiedAdminId(Session session, Integer adminId) {
        Query<SubscriptionType> query = session.createQuery("FROM SubscriptionType s WHERE s.lastModifiedAdmin.id = :adminId", SubscriptionType.class);
        query.setParameter("adminId", adminId);
        return query.getResultList();
    }
}
