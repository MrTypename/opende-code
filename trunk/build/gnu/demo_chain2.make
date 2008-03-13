# C++ Console Executable Makefile autogenerated by premake
# Don't edit this file! Instead edit `premake.lua` then rerun `make`

ifndef CONFIG
  CONFIG=DebugSingleDLL
endif

ifeq ($(CONFIG),DebugSingleDLL)
  BINDIR := ../../lib/DebugSingleDLL
  LIBDIR := ../../lib/DebugSingleDLL
  OBJDIR := obj/chain2/DebugSingleDLL
  OUTDIR := ../../lib/DebugSingleDLL
  CPPFLAGS := -MMD -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -I "../../include" -I "../../ode/src"
  CFLAGS += $(CPPFLAGS) $(TARGET_ARCH) -g
  CXXFLAGS := $(CFLAGS)
  LDFLAGS += -L$(BINDIR) -L$(LIBDIR) -lode_singled -ldrawstuffd -luser32 -lwinmm -lgdi32 -lopengl32 -lglu32
  LDDEPS := ../../lib/DebugSingleDLL/ode_singled.dll ../../lib/DebugSingleDLL/drawstuffd.dll
  RESFLAGS := -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -I "../../include" -I "../../ode/src"
  TARGET := demo_chain2.exe
  BLDCMD = $(CXX) -o $(OUTDIR)/$(TARGET) $(OBJECTS) $(LDFLAGS) $(RESOURCES) $(TARGET_ARCH)
endif

ifeq ($(CONFIG),ReleaseSingleDLL)
  BINDIR := ../../lib/ReleaseSingleDLL
  LIBDIR := ../../lib/ReleaseSingleDLL
  OBJDIR := obj/chain2/ReleaseSingleDLL
  OUTDIR := ../../lib/ReleaseSingleDLL
  CPPFLAGS := -MMD -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -I "../../include" -I "../../ode/src"
  CFLAGS += $(CPPFLAGS) $(TARGET_ARCH) -g
  CXXFLAGS := $(CFLAGS)
  LDFLAGS += -L$(BINDIR) -L$(LIBDIR) -lode_single -ldrawstuff -luser32 -lwinmm -lgdi32 -lopengl32 -lglu32
  LDDEPS := ../../lib/ReleaseSingleDLL/ode_single.dll ../../lib/ReleaseSingleDLL/drawstuff.dll
  RESFLAGS := -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -I "../../include" -I "../../ode/src"
  TARGET := demo_chain2.exe
  BLDCMD = $(CXX) -o $(OUTDIR)/$(TARGET) $(OBJECTS) $(LDFLAGS) $(RESOURCES) $(TARGET_ARCH)
endif

ifeq ($(CONFIG),DebugSingleLib)
  BINDIR := ../../lib/DebugSingleLib
  LIBDIR := ../../lib/DebugSingleLib
  OBJDIR := obj/chain2/DebugSingleLib
  OUTDIR := ../../lib/DebugSingleLib
  CPPFLAGS := -MMD -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -I "../../include" -I "../../ode/src"
  CFLAGS += $(CPPFLAGS) $(TARGET_ARCH) -g
  CXXFLAGS := $(CFLAGS)
  LDFLAGS += -L$(BINDIR) -L$(LIBDIR) -lode_singled -ldrawstuffd -luser32 -lwinmm -lgdi32 -lopengl32 -lglu32
  LDDEPS := ../../lib/DebugSingleLib/libode_singled.a ../../lib/DebugSingleLib/libdrawstuffd.a
  RESFLAGS := -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -I "../../include" -I "../../ode/src"
  TARGET := demo_chain2.exe
  BLDCMD = $(CXX) -o $(OUTDIR)/$(TARGET) $(OBJECTS) $(LDFLAGS) $(RESOURCES) $(TARGET_ARCH)
endif

ifeq ($(CONFIG),ReleaseSingleLib)
  BINDIR := ../../lib/ReleaseSingleLib
  LIBDIR := ../../lib/ReleaseSingleLib
  OBJDIR := obj/chain2/ReleaseSingleLib
  OUTDIR := ../../lib/ReleaseSingleLib
  CPPFLAGS := -MMD -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -I "../../include" -I "../../ode/src"
  CFLAGS += $(CPPFLAGS) $(TARGET_ARCH) -g
  CXXFLAGS := $(CFLAGS)
  LDFLAGS += -L$(BINDIR) -L$(LIBDIR) -lode_single -ldrawstuff -luser32 -lwinmm -lgdi32 -lopengl32 -lglu32
  LDDEPS := ../../lib/ReleaseSingleLib/libode_single.a ../../lib/ReleaseSingleLib/libdrawstuff.a
  RESFLAGS := -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -I "../../include" -I "../../ode/src"
  TARGET := demo_chain2.exe
  BLDCMD = $(CXX) -o $(OUTDIR)/$(TARGET) $(OBJECTS) $(LDFLAGS) $(RESOURCES) $(TARGET_ARCH)
endif

ifeq ($(CONFIG),DebugDoubleDLL)
  BINDIR := ../../lib/DebugDoubleDLL
  LIBDIR := ../../lib/DebugDoubleDLL
  OBJDIR := obj/chain2/DebugDoubleDLL
  OUTDIR := ../../lib/DebugDoubleDLL
  CPPFLAGS := -MMD -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -D "dDOUBLE" -I "../../include" -I "../../ode/src"
  CFLAGS += $(CPPFLAGS) $(TARGET_ARCH) -g
  CXXFLAGS := $(CFLAGS)
  LDFLAGS += -L$(BINDIR) -L$(LIBDIR) -lode_doubled -ldrawstuffd -luser32 -lwinmm -lgdi32 -lopengl32 -lglu32
  LDDEPS := ../../lib/DebugDoubleDLL/ode_doubled.dll ../../lib/DebugDoubleDLL/drawstuffd.dll
  RESFLAGS := -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -D "dDOUBLE" -I "../../include" -I "../../ode/src"
  TARGET := demo_chain2.exe
  BLDCMD = $(CXX) -o $(OUTDIR)/$(TARGET) $(OBJECTS) $(LDFLAGS) $(RESOURCES) $(TARGET_ARCH)
endif

ifeq ($(CONFIG),ReleaseDoubleDLL)
  BINDIR := ../../lib/ReleaseDoubleDLL
  LIBDIR := ../../lib/ReleaseDoubleDLL
  OBJDIR := obj/chain2/ReleaseDoubleDLL
  OUTDIR := ../../lib/ReleaseDoubleDLL
  CPPFLAGS := -MMD -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -D "dDOUBLE" -I "../../include" -I "../../ode/src"
  CFLAGS += $(CPPFLAGS) $(TARGET_ARCH) -g
  CXXFLAGS := $(CFLAGS)
  LDFLAGS += -L$(BINDIR) -L$(LIBDIR) -lode_double -ldrawstuff -luser32 -lwinmm -lgdi32 -lopengl32 -lglu32
  LDDEPS := ../../lib/ReleaseDoubleDLL/ode_double.dll ../../lib/ReleaseDoubleDLL/drawstuff.dll
  RESFLAGS := -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -D "dDOUBLE" -I "../../include" -I "../../ode/src"
  TARGET := demo_chain2.exe
  BLDCMD = $(CXX) -o $(OUTDIR)/$(TARGET) $(OBJECTS) $(LDFLAGS) $(RESOURCES) $(TARGET_ARCH)
endif

ifeq ($(CONFIG),DebugDoubleLib)
  BINDIR := ../../lib/DebugDoubleLib
  LIBDIR := ../../lib/DebugDoubleLib
  OBJDIR := obj/chain2/DebugDoubleLib
  OUTDIR := ../../lib/DebugDoubleLib
  CPPFLAGS := -MMD -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -D "dDOUBLE" -I "../../include" -I "../../ode/src"
  CFLAGS += $(CPPFLAGS) $(TARGET_ARCH) -g
  CXXFLAGS := $(CFLAGS)
  LDFLAGS += -L$(BINDIR) -L$(LIBDIR) -lode_doubled -ldrawstuffd -luser32 -lwinmm -lgdi32 -lopengl32 -lglu32
  LDDEPS := ../../lib/DebugDoubleLib/libode_doubled.a ../../lib/DebugDoubleLib/libdrawstuffd.a
  RESFLAGS := -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -D "dDOUBLE" -I "../../include" -I "../../ode/src"
  TARGET := demo_chain2.exe
  BLDCMD = $(CXX) -o $(OUTDIR)/$(TARGET) $(OBJECTS) $(LDFLAGS) $(RESOURCES) $(TARGET_ARCH)
endif

ifeq ($(CONFIG),ReleaseDoubleLib)
  BINDIR := ../../lib/ReleaseDoubleLib
  LIBDIR := ../../lib/ReleaseDoubleLib
  OBJDIR := obj/chain2/ReleaseDoubleLib
  OUTDIR := ../../lib/ReleaseDoubleLib
  CPPFLAGS := -MMD -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -D "dDOUBLE" -I "../../include" -I "../../ode/src"
  CFLAGS += $(CPPFLAGS) $(TARGET_ARCH) -g
  CXXFLAGS := $(CFLAGS)
  LDFLAGS += -L$(BINDIR) -L$(LIBDIR) -lode_double -ldrawstuff -luser32 -lwinmm -lgdi32 -lopengl32 -lglu32
  LDDEPS := ../../lib/ReleaseDoubleLib/libode_double.a ../../lib/ReleaseDoubleLib/libdrawstuff.a
  RESFLAGS := -D "_CRT_SECURE_NO_DEPRECATE" -D "WIN32" -D "dDOUBLE" -I "../../include" -I "../../ode/src"
  TARGET := demo_chain2.exe
  BLDCMD = $(CXX) -o $(OUTDIR)/$(TARGET) $(OBJECTS) $(LDFLAGS) $(RESOURCES) $(TARGET_ARCH)
endif

OBJECTS := \
	$(OBJDIR)/demo_chain2.o \

RESOURCES := \
	$(OBJDIR)/resources.res \

MKDIR_TYPE := msdos
CMD := $(subst \,\\,$(ComSpec)$(COMSPEC))
ifeq (,$(CMD))
  MKDIR_TYPE := posix
endif
ifeq (/bin/sh.exe,$(SHELL))
  MKDIR_TYPE := posix
endif
ifeq ($(MKDIR_TYPE),posix)
  CMD_MKBINDIR := mkdir -p $(BINDIR)
  CMD_MKLIBDIR := mkdir -p $(LIBDIR)
  CMD_MKOUTDIR := mkdir -p $(OUTDIR)
  CMD_MKOBJDIR := mkdir -p $(OBJDIR)
else
  CMD_MKBINDIR := $(CMD) /c if not exist $(subst /,\\,$(BINDIR)) mkdir $(subst /,\\,$(BINDIR))
  CMD_MKLIBDIR := $(CMD) /c if not exist $(subst /,\\,$(LIBDIR)) mkdir $(subst /,\\,$(LIBDIR))
  CMD_MKOUTDIR := $(CMD) /c if not exist $(subst /,\\,$(OUTDIR)) mkdir $(subst /,\\,$(OUTDIR))
  CMD_MKOBJDIR := $(CMD) /c if not exist $(subst /,\\,$(OBJDIR)) mkdir $(subst /,\\,$(OBJDIR))
endif

.PHONY: clean

$(OUTDIR)/$(TARGET): $(OBJECTS) $(LDDEPS) $(RESOURCES)
	@echo Linking demo_chain2
	-@$(CMD_MKBINDIR)
	-@$(CMD_MKLIBDIR)
	-@$(CMD_MKOUTDIR)
	@$(BLDCMD)

clean:
	@echo Cleaning demo_chain2
ifeq ($(MKDIR_TYPE),posix)
	-@rm -f $(OUTDIR)/$(TARGET)
	-@rm -rf $(OBJDIR)
else
	-@if exist $(subst /,\,$(OUTDIR)/$(TARGET)) del /q $(subst /,\,$(OUTDIR)/$(TARGET))
	-@if exist $(subst /,\,$(OBJDIR)) del /q $(subst /,\,$(OBJDIR))
	-@if exist $(subst /,\,$(OBJDIR)) rmdir /s /q $(subst /,\,$(OBJDIR))
endif

$(OBJDIR)/demo_chain2.o: ../../ode/demo/demo_chain2.cpp
	-@$(CMD_MKOBJDIR)
	@echo $(notdir $<)
	@$(CXX) $(CXXFLAGS) -o $@ -c $<

$(OBJDIR)/resources.res: ../../drawstuff/src/resources.rc
	-@$(CMD_MKOBJDIR)
	@echo $(notdir $<)
	@windres $< -O coff -o $@ $(RESFLAGS)

-include $(OBJECTS:%.o=%.d)

