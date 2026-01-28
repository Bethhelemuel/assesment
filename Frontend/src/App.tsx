import { Navigate, Route, Routes } from 'react-router-dom'
import SidebarLayout from './pages/Sidebar'
import Dashboard from './pages/Dashboard'
import Users from './pages/Users'
import Groups from './pages/Groups'
import Permissions from './pages/Permissions'

function App() { 
  return (
    <SidebarLayout>
      <Routes>
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/users" element={<Users />} />
        <Route path="/groups" element={<Groups />} />
        <Route path="/permissions" element={<Permissions />} />
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </SidebarLayout> 
  )
}

export default App
