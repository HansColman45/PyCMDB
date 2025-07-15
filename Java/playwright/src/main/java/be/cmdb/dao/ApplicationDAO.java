package be.cmdb.dao;

import be.cmdb.model.Application;
import org.hibernate.Session;
import org.hibernate.query.Query;
import java.util.List;

public class ApplicationDAO implements BaseDAO<Application,Integer> {
    @Override
    public Application save(Session session, Application entity) {
        session.persist(entity);
        return entity;
    }

    @Override
    public Application update(Session session, Application entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public Application findById(Session session, Integer id) {
        return session.find(Application.class, id);
    }

    @Override
    public List<Application> findAll(Session session) {
        Query<Application> query = session.createQuery("FROM Application", Application.class);
        return query.getResultList();
    }

    @Override
    public void delete(Session session, Application entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, Integer id) {
        Application application = findById(session, id);
        if (application != null) {
            delete(session, application);
        } else {
            // Optionally handle the case where the application is not found
            System.out.println("Application with ID " + id + " not found.");
        }
    }

    @Override
    public long count(Session session) {
        Query<Long> query = session.createQuery("SELECT COUNT(a) FROM Application a", Long.class);
        return query.getSingleResult();
    }

    /**
     * Finds applications by their name.
     * @param session the Hibernate session
     * @param name the name of the application to search for
     * @return a list of applications with the specified name
     */
    public List<Application> findByName(Session session, String name) {
        Query<Application> query = session.createQuery("FROM Application WHERE name = :name", Application.class);
        query.setParameter("name", name);
        return query.getResultList();
    }
}
