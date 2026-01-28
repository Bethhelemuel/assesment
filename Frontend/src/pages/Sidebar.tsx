import type { FC, ReactNode } from 'react'
import { NavLink } from 'react-router-dom'
import logo from '@/assets/logo.png' 
import { Home, User, Users, Shield } from 'lucide-react'

 
interface SidebarLayoutProps {
  children: ReactNode
}

const linkBaseClasses =
  'flex items-center gap-3 px-5 py-3 rounded-md text-base font-medium hover:bg-gray-200 hover:text-black border-l-4 border-transparent'

const SidebarLayout: FC<SidebarLayoutProps> = ({ children }) => {
  return (
    <div className="min-h-screen flex bg-background text-foreground">
      <aside className="w-56 border-r bg-secondary/40 p-4 space-y-4">
        <div className="flex items-center justify-center mb-6">
          <img src={logo} alt="Logo" className="h-7 w-auto" />
        </div>
        <nav className="space-y-2">
          <NavLink 
            to="/dashboard"
            className={({ isActive }: { isActive: boolean }) =>
              isActive
                ? `${linkBaseClasses} bg-primary text-white font-semibold border-primary hover:bg-gray-200 hover:text-black` 
                : `${linkBaseClasses}`
            }
          >
            <Home className="h-4 w-4" />
            <span>Home</span>
          </NavLink>
          <NavLink
            to="/users"
            className={({ isActive }: { isActive: boolean }) =>
              isActive
                ? `${linkBaseClasses} bg-primary text-white font-semibold border-primary hover:bg-gray-200 hover:text-black`
                : `${linkBaseClasses}`
            }
          >
            <User className="h-4 w-4" />
            <span>Users</span>
          </NavLink>
          <NavLink
            to="/groups"
            className={({ isActive }: { isActive: boolean }) =>
              isActive
                ? `${linkBaseClasses} bg-primary text-white font-semibold border-primary hover:bg-gray-200 hover:text-black`
                : `${linkBaseClasses}`
            }
          >
            <Users className="h-4 w-4" />
            <span>Groups</span>
          </NavLink>
          <NavLink
            to="/permissions"
            className={({ isActive }: { isActive: boolean }) =>
              isActive
                ? `${linkBaseClasses} bg-primary text-white font-semibold border-primary hover:bg-gray-200 hover:text-black`
                : `${linkBaseClasses}`
            }
          >
            <Shield className="h-4 w-4" />
            <span>Permissions</span>
          </NavLink>
        </nav>
      </aside>
      <main className="flex-1 p-6">{children}</main>
    </div>
  )
}

export default SidebarLayout
