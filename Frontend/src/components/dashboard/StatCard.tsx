import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';

interface StatCardProps {
  title: string;
  value: number | string;
  description: string;
  icon: React.ReactNode;
  loading?: boolean;
}

export const StatCard = ({ title, value, description, icon, loading = false }: StatCardProps) => {
  return (
    <Card className="bg-white border-red-200 shadow-sm">
      <CardHeader>
        <CardDescription className="text-red-600 text-sm font-medium uppercase tracking-wider">
          {title}
        </CardDescription>
        <CardTitle className="text-6xl font-bold text-gray-900 mt-4">
          {loading ? "..." : value}
        </CardTitle>
      </CardHeader>
      <CardContent>
        <div className="flex items-center gap-2 text-gray-600">
          {icon}
          <span className="text-sm">{description}</span>
        </div>
      </CardContent>
    </Card>
  );
};
