import { useEffect, useState } from "react";
import { StatCard, InfoCard, UsersIcon, GroupsIcon, PermissionsIcon, TrendingUpIcon, ChartIcon, BarChartIcon, LockIcon } from "@/components/dashboard";
import { dashboardService } from "../service/dashboardService";

const Dashboard = () => {
  const [stats, setStats] = useState({
    totalUsers: 0,
    totalGroups: 0,
    totalPermissions: 0,
    mostAssignedGroup: "N/A",
    mostCommonPermission: "N/A",
  });

  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchDashboard = async () => {
      try {
        setLoading(true);
        const data = await dashboardService.getDashboard();
        setStats(data);
      } catch (error) {
        console.error("Error fetching dashboard data:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchDashboard();
  }, []);

  return (
    <div className="min-h-screen bg-white p-8">
      {/* Header */}
      <div className="max-w-7xl mx-auto mb-12">
        <p className="text-gray-600 text-lg"></p>
      </div>

      {/* Stats Grid */}
      <div className="max-w-7xl mx-auto grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
        <StatCard
          title="Total Users"
          value={stats.totalUsers}
          description="Active accounts"
          icon={<UsersIcon />}
          loading={loading}
        />

        <StatCard
          title="Total Groups"
          value={stats.totalGroups}
          description="Organization units"
          icon={<GroupsIcon />}
          loading={loading}
        />

        <StatCard
          title="Total Permissions"
          value={stats.totalPermissions}
          description="Access controls"
          icon={<PermissionsIcon />}
          loading={loading}
        />
      </div>

      {/* Detailed Info Cards */}
      <div className="max-w-7xl mx-auto grid grid-cols-1 md:grid-cols-2 gap-6">
        <InfoCard
          title="Most Assigned Group"
          value={stats.mostAssignedGroup}
          subtitle="Highest user allocation"
          headerIcon={<TrendingUpIcon />}
          icon={<ChartIcon />}
          loading={loading}
        />

        <InfoCard
          title="Most Common Permission"
          value={stats.mostCommonPermission}
          subtitle="Frequently granted access"
          headerIcon={<BarChartIcon />}
          icon={<LockIcon />}
          loading={loading}
          valueColor="text-gray-400"
          iconBg="bg-gray-200"
        />
      </div>
    </div>
  );
};

export default Dashboard;
