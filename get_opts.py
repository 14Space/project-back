import json
import sqlite3
import pyodbc

conn_str = r'DRIVER={ODBC Driver 17 for SQL Server};SERVER=localhost;DATABASE=FrameDb;Trusted_Connection=yes;'
conn = pyodbc.connect(conn_str)
cursor = conn.cursor()
cursor.execute('''SELECT c.Name as Category, a.Options 
                 FROM Attributes a JOIN Categories c ON a.CategoryId = c.Id 
                 WHERE a.Name LIKE '%Подкатегори%' AND a.Name LIKE '%/%'
                 ORDER BY c.Name''')
for row in cursor.fetchall():
    opts = json.loads(row.Options)
    ru_opts = [o.split(' / ')[0] for o in opts]
    print(f"{row.Category}: {ru_opts}")
