using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Advent_of_Code_2020.StatsBuilder {
	public class AoCLeaderboard {
		public string _event { get; set; }
		public string owner_id { get; set; }
		public Dictionary<string, AoCUser> members { get; set; }
	}
}