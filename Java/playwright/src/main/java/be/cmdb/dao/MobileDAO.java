package be.cmdb.dao;

import be.cmdb.model.Mobile;
import org.hibernate.Session;
import org.hibernate.query.Query;
import java.util.List;

public class MobileDAO implements BaseDAO<Mobile, Integer> {
    @Override
    public Mobile save(Session session, Mobile entity) {
        session.persist(entity);
        return entity;
    }

    @Override
    public Mobile update(Session session, Mobile entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public Mobile findById(Session session, Integer id) {
        return session.find(Mobile.class, id);
    }

    @Override
    public List<Mobile> findAll(Session session) {
        Query<Mobile> query = session.createQuery("FROM Mobile", Mobile.class);
        return query.getResultList();
    }

    @Override
    public void delete(Session session, Mobile entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, Integer id) {
        Mobile mobile = findById(session, id);
        if (mobile != null) {
            delete(session, mobile);
        } else {
            // Optionally handle the case where the mobile is not found
            System.out.println("Mobile with ID " + id + " not found.");
        }
    }

    @Override
    public long count(Session session) {
        Query<Long> query = session.createQuery("SELECT COUNT(m) FROM Mobile m", Long.class);
        return query.getSingleResult();
    }

    /**
     * Finds mobiles by the last modified admin ID.
     * @param session the Hibernate session
     * @param adminId the ID of the admin who last modified the mobile
     * @return a list of mobiles modified by the specified admin
     */
    public List<Mobile> findByLastModifiedAdminId(Session session, Integer adminId) {
        Query<Mobile> query = session.createQuery("FROM Mobile m WHERE m.lastModifiedAdminId = :adminId", Mobile.class);
        query.setParameter("adminId", adminId);
        return query.getResultList();
    }
}
