fs =require("fs")

tag = "IntersectGeom3Exp2xxx"
  const exp = [
		    [[9, 9, 8], [9, 9, 9], [9, 8, 9], [9, 8, 8]],
    [[8, 9, 9], [9, 9, 9], [9, 9, 8], [8, 9, 8]],
    [[9, 8, 9], [9, 9, 9], [8, 9, 9], [8, 8, 9]],
    [[8, 9, 9], [8, 9, 8], [8, 8, 8], [8, 8, 9]],
    [[8, 8, 9], [8, 8, 8], [9, 8, 8], [9, 8, 9]],
    [[9, 8, 8], [8, 8, 8], [8, 9, 8], [9, 9, 8]]

  ]

if (exp[0][0].length === 2) {
	asides(exp)
} else if (exp[0].length === 2 && exp[0][0].length === undefined) {
	av2(exp)
} else {
	llv3(exp)
}

function av2(tree) {
	data = ""
	let indent_level = 0
	function indent() {
		indent_level++
	}
	function dedent() {
		indent_level--
	}
	function tab() {
		for(let i=0; i<indent_level; i++) data += "  ";
	}

	data += "namespace CSharpCADTests;\n"
	data += "\n"
	data += "using NUnit.Framework;\n"
	data += "\n"
	data += "public static partial class UnitTestData {\n"
	indent();
		tab(); data += `public static Vec2[] ${tag} = new Vec2[] {\n`
		indent();
		for (let i=0; i<tree.length; i++) {
			let sub = tree[i]
				let pts = tree[i]
				tab(); data += `new Vec2(${pts[0]}, ${pts[1]})${i<tree.length-1?",":""}\n`
		}
		dedent();
	tab(); data += "};\n"
	dedent();
	data += "}\n"

	fs.writeFile("src/unit_test_data/"+tag+".cs", data, (err) => console.log(err?err:`UnitTestData.${tag} Success.`))
}

function asides(tree) {
	data = ""
	let indent_level = 0
	function indent() {
		indent_level++
	}
	function dedent() {
		indent_level--
	}
	function tab() {
		for(let i=0; i<indent_level; i++) data += "  ";
	}

	data += "namespace CSharpCADTests;\n"
	data += "\n"
	data += "using NUnit.Framework;\n"
	data += "\n"
	data += "public static partial class UnitTestData {\n"
	indent();
		tab(); data += `internal static Geom2.Side[] ${tag} = new Geom2.Side[] {\n`
		indent();
		for (let i=0; i<tree.length; i++) {
			let sub = tree[i]
			indent();
			tab(); data += "new Geom2.Side("
			for (let j=0; j<sub.length; j++) {
				let pts = sub[j]
				data += `new Vec2(${pts[0]}, ${pts[1]})${j<sub.length-1?", ":""}`
			}
			data += `)${i<tree.length-1?",":""}\n`
			dedent();
		}
		dedent();
	tab(); data += "};\n"
	dedent();
	data += "}\n"

	fs.writeFile("src/unit_test_data/"+tag+".cs", data, (err) => console.log(err?err:`UnitTestData.${tag} Success.`))
}

function llv3(tree) {
	data = ""
	let indent_level = 0
	function indent() {
		indent_level++
	}
	function dedent() {
		indent_level--
	}
	function tab() {
		for(let i=0; i<indent_level; i++) data += "  ";
	}

	data += "namespace CSharpCADTests;\n"
	data += "\n"
	data += "using NUnit.Framework;\n"
	data += "\n"
	data += "public static partial class UnitTestData {\n"
	indent();
		tab(); data += `public static List<List<Vec3>> ${tag} = new List<List<Vec3>> {\n`
		indent();
		for (let i=0; i<tree.length; i++) {
			let sub = tree[i]
			indent();
			tab(); data += "new List<Vec3> {\n"
			for (let j=0; j<sub.length; j++) {
				let pts = sub[j]
				indent();
					tab(); data += `new Vec3(${pts[0]}, ${pts[1]}, ${pts[2]})${j<sub.length-1?",":""}\n`
				dedent();
			}
			dedent();
			tab(); data += `}${i<tree.length-1?",":""}\n`
		}
		dedent();
	tab(); data += "};\n"
	dedent();
	data += "}\n"

	fs.writeFile("src/unit_test_data/"+tag+".cs", data, (err) => console.log(err?err:`UnitTestData.${tag} Success.`))
}
