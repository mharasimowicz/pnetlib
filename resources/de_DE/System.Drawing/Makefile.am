
.PHONY: System.Drawing.resources.dll

all-local: System.Drawing.resources.dll

System.Drawing.resources.dll:
	"$(CSANT)" $(CSANT_FLAGS) -b "$(srcdir)" -B . \
			-f "$(srcdir)/resources.build" all

install-data-local:
	@$(NORMAL_INSTALL)
	$(mkinstalldirs) $(DESTDIR)$(libdir)/cscc/lib
	"$(CSANT)" $(CSANT_FLAGS) -b "$(srcdir)" -B . \
			-f "$(srcdir)/resources.build" -i all

uninstall-local:
	@$(NORMALL_UNINSTALL)
	"$(CSANT)" $(CSANT_FLAGS) -b "$(srcdir)" -B . \
			-f "$(srcdir)/resources.build" -u all

CLEANFILES = System.Drawing.de.resources \
			 System.Drawing.resources.dll
