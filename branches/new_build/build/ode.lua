package.name = "ode"
package.path = options["target"]
package.language = "c++"
package.objdir = "obj/ode"


-- Write a custom <config.h> to include/ode, based on the specified flags

  io.input("config.in")
  local text = io.read("*a")
  
  if (options["with-doubles"]) then
    text = string.gsub(text, "{PRECISION}", "dDOUBLE")
  else
    text = string.gsub(text, "{PRECISION}", "dSINGLE")
  end
  
  if (options["no-cylinder"]) then
    text = string.gsub(text, "{CYLINDER}", "0")
  else
    text = string.gsub(text, "{CYLINDER}", "1")
  end
    
  if (options["no-trimesh"]) then
    text = string.gsub(text, "{TRIMESH}", "0")
  else
    text = string.gsub(text, "{TRIMESH}", "1")
  end
    
  io.output("../include/ode/config.h")
  io.write(text)
  io.close()

  
-- Package Build Settings

  package.config["DebugDLL"].kind = "dll"
  package.config["DebugLib"].kind = "lib"
  package.config["ReleaseDLL"].kind = "dll"
  package.config["ReleaseLib"].kind = "lib"

  table.insert(package.config["DebugDLL"].defines, "ODE_DLL")
  table.insert(package.config["ReleaseDLL"].defines, "ODE_DLL")
  
  table.insert(package.config["DebugLib"].defines, "ODE_LIB")
  table.insert(package.config["ReleaseLib"].defines, "ODE_LIB")
  
  package.includepaths =
  {
    "../../include",
    "../../OPCODE"
  }
 
  if (windows) then
    table.insert(package.defines, "WIN32")
  end
 
 -- disable VS2005 CRT security warnings
  if (options["target"] == "vs2005") then
    table.insert(package.defines, "_CRT_SECURE_NO_DEPRECATE")
  end
  

-- Libraries

  if (windows) then
    table.insert(package.links, "user32")
  end

    
-- Files

  core_files =
  {
    matchfiles("../../include/ode/*.h"),
    matchfiles ("../../ode/src/*.h", "../../ode/src/*.c", "../../ode/src/*.cpp")
  }

  excluded_files =
  {
    "../../ode/src/scrapbook.cpp",
    "../../ode/src/stack.cpp"
  }

  trimesh_files =
  {
    "../../ode/src/collision_trimesh_internal.h",
    "../../ode/src/collision_trimesh.cpp",
    "../../ode/src/collision_trimesh_box.cpp",
    "../../ode/src/collision_trimesh_ccylinder.cpp",
    "../../ode/src/collision_cylinder_trimesh.cpp",
    "../../ode/src/collision_trimesh_distance.cpp",
    "../../ode/src/collision_trimesh_ray.cpp",
    "../../ode/src/collision_trimesh_sphere.cpp",
    "../../ode/src/collision_trimesh_trimesh.cpp"
  }
  
  opcode_files =
  {
    matchrecursive("../../OPCODE/*.h", "../../OPCODE/*.cpp")
  }
  
  cylinder_files =
  {
    "../../ode/src/collision_cylinder_box.cpp",
    "../../ode/src/collision_cylinder_plane.cpp",
    "../../ode/src/collision_cylinder_sphere.cpp",
    "../../ode/src/collision_cylinder_trimesh.cpp",
  }
   
  dif_files = 
  {
    "../../ode/src/export-dif.cpp"
  }

  package.files = { core_files } 
  package.excludes = { excluded_files }
 
  if (options["no-dif"]) then
    table.insert(package.excludes, dif_files)
  end

  if (options["no-cylinder"]) then
    table.insert(package.excludes, cylinder_files)
  end
  
  if (options["no-trimesh"]) then
    table.insert(package.excludes, trimesh_files)
  else
    table.insert(package.files, opcode_files)
  end
 