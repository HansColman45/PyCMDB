package be.cmdb.dao;

import be.cmdb.model.Subscription;
import org.hibernate.Session;
import org.hibernate.query.Query;
import java.util.List;

public class SubscriptionDAO implements BaseDAO<Subscription, Integer> {

    @Override
    public Subscription save(Session session, Subscription entity) {
        session.persist(entity);
        return entity;
    }

    @Override
    public Subscription update(Session session, Subscription entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public Subscription findById(Session session, Integer id) {
        return session.find(Subscription.class, id);
    }

    @Override
    public List<Subscription> findAll(Session session) {
        Query<Subscription> query = session.createQuery("FROM Subscription", Subscription.class);
        return query.getResultList();
    }

    @Override
    public void delete(Session session, Subscription entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, Integer id) {
        Subscription subscription = findById(session,id);
        if (subscription != null) {
            delete(session, subscription);
        } else {
            // Optionally handle the case where the subscription is not found
            System.out.println("Subscription with ID " + id + " not found.");
        }
    }

    @Override
    public long count(Session session) {
        Query<Long> query = session.createQuery("SELECT COUNT(s) FROM Subscription s", Long.class);
        return query.getSingleResult();
    }

    public List<Subscription> findByLastModifiedAdminId(Session session, Integer adminId) {
        Query<Subscription> query = session.createQuery("FROM Subscription s WHERE s.lastModifiedAdmin.id = :adminId", Subscription.class);
        query.setParameter("adminId", adminId);
        return query.getResultList();
    }
}
