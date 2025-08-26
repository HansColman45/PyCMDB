insert into log (RolePermId, LogText, LogDate)
select rp.Id RolePerm, CONCAT('Permission for level ',rp.Level, ' on ',m.Label, ' to ',p.Rights, ' Created by SQL import') as LogText, '2012-01-01 00:00:00' logDate
from RolePerm rp
join Menu m on rp.MenuId = m.MenuId
join Permission p on rp.PermissionId = p.Id;