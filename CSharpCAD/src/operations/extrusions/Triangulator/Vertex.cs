namespace CSharpCAD.Triangulator
{
	internal struct Vertex
	{
		public readonly Vec2 Position;
		public readonly int Index;

		public Vertex(Vec2 position, int index)
		{
			Position = position;
			Index = index;
		}

		public override bool Equals(object? obj)
		{
			if (obj is null) return false;
            if (obj.GetType() != typeof(Vertex)) 
				return false;
			return Equals((Vertex)obj);
		}

		public bool Equals(Vertex obj)
		{
			return obj.Position.Equals(Position) && obj.Index == Index;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Position.GetHashCode() * 397) ^ Index;
			}
		}

		public override string ToString()
		{
			return string.Format("{0} ({1})", Position, Index);
		}
	}
}
